FolderTree = function (clientId, uniqueId, expandImage, collapseImage, noExpandImage) {
	this._clientId = clientId;
	this._uniqueId = uniqueId;
	this._expandImage = expandImage;
	this._collapseImage = collapseImage;
	this._noExpandImage = noExpandImage;
	this._selectedNode = null;
}

FolderTree.prototype = {
	ToggleExpand : function (nodeId) {
		this._ToggleExpand(nodeId, this.PopulateCallback, null, this.PopulateError);
	},
	
	_ToggleExpand : function (nodeId, eventCallback, externalContext, errorCallback) {
		var node = this._getNode (nodeId);
		if(!node) return true;

		var nodeSpan = this._getChildNodesSpan (nodeId);
		var expand = node._expand || node._refresh || nodeSpan.style.display == "none";

		if(expand && node._populating) return false;
		
		if (node._refresh || (nodeSpan.style.display == "none" && nodeSpan.innerHTML.length == 0)) {
			node._populating = true;
			var nodePath = node.attributes["nodepath"].value;
			this.PopulateNode (nodePath, eventCallback, {'folderTree':this,'node':node,'nodeSpan':nodeSpan,'nodeId':nodeId,'nodePath':nodePath,'externalContext':externalContext}, errorCallback);
			return false;
		}
		
		node._expand = false;
		
		var image = this._getNodeImg (nodeId);
		if(nodeSpan.innerHTML == "*") {
			image.src = this._noExpandImage;
			nodeSpan.style.display = "none";
		} else {
			nodeSpan.style.display = expand ? "block" : "none";
			image.src = expand ? this._collapseImage : this._expandImage;
		}
		return true;
	},

	PopulateError : function (data, ctx){
		ctx.folderTree.PopulateCallback (data, ctx);
	},

	PopulateCallback : function (data, ctx)
	{
		ctx.node._refresh = false;
		ctx.node._populating = false;
		if(data == "*")
			ctx.nodeSpan.style.display = "none";
		ctx.nodeSpan.innerHTML = data;
		ctx.folderTree._ToggleExpand (ctx.nodeId, null, null, null);
	},
	
	Navigate : function (nodes, n) {
		for(var i=n; i<nodes.length-1; i++) {
			var node = this._getNode (nodes[i]);
			if (!node) return;
			if(!node._refresh) {
				var nodeSpan = this._getChildNodesSpan (nodes[i]);
				if (nodeSpan.style.display == "block") continue;
				if (nodeSpan.innerHTML == "*") return;
			}
			if(!this._ToggleExpand(nodes[i], this.NavigateCallback, {'nodes':nodes,'index':i}, this.PopulateError)) return;
		}
		var nodeId = nodes[nodes.length-1];
		var node = this._getNode (nodeId);
		this.SelectNode (nodeId);
		if(node._refresh)
			this.Refresh (nodeId);
	},

	NavigateCallback : function (data, ctx)
	{
		ctx.folderTree.PopulateCallback (data, ctx);
		var interval = window.setInterval(function() {
			window.clearInterval(interval);
			ctx.folderTree.Navigate (ctx.externalContext.nodes, ctx.externalContext.index);
		}, 0);
	},
	
	Refresh : function (nodeId) {
		var node = this._getNode (nodeId);
		if(!node) return;
		var nodeSpan = this._getChildNodesSpan (nodeId);
		var expand = nodeSpan.style.display == "block" || nodeSpan.innerHTML == "*";
		
		if(expand) {
			node._expand = true;
			node._refresh = true;
			this.ToggleExpand (nodeId);
		} else {
			nodeSpan.innerHTML = "";
			nodeSpan.style.display = "none"
		}
	},
	
	RequireRefresh : function (nodes) {
		for(var i=0; i<nodes.length; i++) {
			var node = this._getNode (nodes[i]);
			if(!node) continue;
			node._expand = true;
			node._refresh = true;
		}
	},
	
	_getNode : function (nodeId) {
		var id = this._clientId + "_" + nodeId + "_node";
		return WebForm_GetElementById (id);
	},
	
	_getNodeImg : function (nodeId) {
		var imgId = this._clientId + "_" + nodeId + "_img";
		return WebForm_GetElementById (imgId);
	},
	
	_getChildNodesSpan : function (nodeId) {
		var spanId = this._clientId + "_" + nodeId + "_span";
		return WebForm_GetElementById (spanId);
	},
	
	_getNodeLink : function (node) { return node.childNodes[node.childNodes.length - 1]; },

	SelectNode : function (nodeId) {
		var node = this._getNode (nodeId);
		if(!node) return;
		if(node === this._selectedNode)
			return;
		
		if(this._selectedNode !== null)
			this._restoreStyle (this._selectedNode);
		
		this._selectedNode = node;
		this._appendStyle (node, this._selectedClass, this._selectedLinkClass);	
	},
	
	HoverNode : function (div)	{
		var node = WebForm_GetElementById (div.id+"_node");
		if(node === this._selectedNode)
			return;
		this._appendStyle (node, this._hoverClass, this._hoverLinkClass);	
	},

	UnhoverNode : function (div) {
		var node = WebForm_GetElementById (div.id+"_node");
		if(node === this._selectedNode)
			return;
		this._restoreStyle (node);
	},
	
	_appendStyle : function (node, nodeClass, nodeLinkClass) {
		if (nodeClass) {
			if (!node._normalClass)
				node._normalClass = node.className;
			node.className = node._normalClass + " " + nodeClass;
		}
		if (nodeLinkClass) {
			var nodeLink = this._getNodeLink(node);
			if(!nodeLink) return;
			if (!nodeLink._normalClass)
				nodeLink._normalClass = nodeLink.className;
			nodeLink.className = nodeLink._normalClass + " " + nodeLinkClass;
		}
	},

	_restoreStyle : function (node) {
		if (node._normalClass)
			node.className = node._normalClass;
		var nodeLink = this._getNodeLink(node);
		if (nodeLink && nodeLink._normalClass)
			nodeLink.className = nodeLink._normalClass;
	}
}
