var FolderTree = function (controllerId, clientId, uniqueId, expandImage, collapseImage, noExpandImage) {
    this._clientId = clientId;
    this._uniqueId = uniqueId;
    this._expandImage = expandImage;
    this._collapseImage = collapseImage;
    this._noExpandImage = noExpandImage;
    this._selectedNode = null;
    this._controller = eval('WFM_' + controllerId);
};
FolderTree.prototype = {
    getController: function () {
        return this._controller;
    },
    ToggleExpand: function (nodeId) {
        this._ToggleExpand(nodeId, this.PopulateCallback, null, this.PopulateError);
    },
    _ToggleExpand: function (nodeId, eventCallback, externalContext, errorCallback) {
        var node = this._getNode(nodeId);
        if(!node) {
            return true;
        }
        var nodeSpan = this._getChildNodesSpan(nodeId);
        var expand = node._expand || node._refresh || nodeSpan.style.display == "none";
        if(expand && node._populating) {
            return false;
        }
        if(node._refresh || (nodeSpan.style.display == "none" && nodeSpan.innerHTML.length == 0)) {
            node._populating = true;
            var nodePath = node.attributes["nodepath"].value;
            this.PopulateNode(nodePath, eventCallback, {
                'folderTree': this,
                'node': node,
                'nodeSpan': nodeSpan,
                'nodeId': nodeId,
                'nodePath': nodePath,
                'externalContext': externalContext
            }, errorCallback);
            return false;
        }
        node._expand = false;
        var image = this._getNodeImg(nodeId);
        if(nodeSpan.innerHTML == "*") {
            image.src = this._noExpandImage;
            nodeSpan.style.display = "none";
        } else {
            nodeSpan.style.display = expand ? "block" : "none";
            image.src = expand ? this._collapseImage : this._expandImage;
        }
        return true;
    },
    PopulateError: function (data, ctx) {
        ctx.folderTree.PopulateCallback(data, ctx);
    },
    PopulateCallback: function (data, ctx) {
        ctx.node._refresh = false;
        ctx.node._populating = false;
        if(data == "*") {
            ctx.nodeSpan.style.display = "none";
        }
        ctx.nodeSpan.innerHTML = data;
        ctx.folderTree._ToggleExpand(ctx.nodeId, null, null, null);
    },
    Navigate: function (nodes, n) {
        for(var i = n; i < nodes.length - 1; i++) {
            var node = this._getNode(nodes[i]);
            if(!node) {
                return;
            }
            if(!node._refresh) {
                var nodeSpan = this._getChildNodesSpan(nodes[i]);
                if(nodeSpan.style.display == "block") {
                    continue;
                }
                if(nodeSpan.innerHTML == "*") {
                    return;
                }
            }
            if(!this._ToggleExpand(nodes[i], this.NavigateCallback, {
                'nodes': nodes,
                'index': i
            }, this.PopulateError)) {
                return;
            }
        }
        var nodeId = nodes[nodes.length - 1];
        var node = this._getNode(nodeId);
        if(!node) {
            return;
        }
        this.SelectNode(nodeId);
        if(node._refresh) {
            this.Refresh(nodeId);
        }
    },
    NavigateCallback: function (data, ctx) {
        ctx.folderTree.PopulateCallback(data, ctx);
        var interval = window.setInterval(function () {
            window.clearInterval(interval);
            ctx.folderTree.Navigate(ctx.externalContext.nodes, ctx.externalContext.index);
        }, 0);
    },
    Refresh: function (nodeId) {
        var node = this._getNode(nodeId);
        if(!node) {
            return;
        }
        var nodeSpan = this._getChildNodesSpan(nodeId);
        var expand = nodeSpan.style.display == "block" || nodeSpan.innerHTML == "*";
        if(expand) {
            node._expand = true;
            node._refresh = true;
            this.ToggleExpand(nodeId);
        } else {
            nodeSpan.innerHTML = "";
            nodeSpan.style.display = "none";
        }
    },
    RequireRefresh: function (nodes) {
        for(var i = 0; i < nodes.length; i++) {
            var node = this._getNode(nodes[i]);
            if(!node) {
                continue;
            }
            node._expand = true;
            node._refresh = true;
        }
    },
    _getNode: function (nodeId) {
        var id = this._clientId + "_" + nodeId + "_node";
        return WebForm_GetElementById(id);
    },
    _getNodeImg: function (nodeId) {
        var imgId = this._clientId + "_" + nodeId + "_img";
        return WebForm_GetElementById(imgId);
    },
    _getChildNodesSpan: function (nodeId) {
        var spanId = this._clientId + "_" + nodeId + "_span";
        return WebForm_GetElementById(spanId);
    },
    _getNodeLink: function (node) {
        return node.childNodes[node.childNodes.length - 1];
    },
    _getImageLink: function (node) {
        return node.previousSibling.childNodes[node.childNodes.length - 1];
    },
    SelectNode: function (nodeId) {
        var node = this._getNode(nodeId);
        if(!node) {
            return;
        }
        if(node === this._selectedNode) {
            return;
        }
        if(this._selectedNode !== null) {
            this._restoreStyle(this._selectedNode);
        }
        this._selectedNode = node;
        this._appendStyle(node, this._selectedClass, this._selectedLinkClass);
    },
    HoverNode: function (div, e) {
        var e = e || window.event;
        var node = WebForm_GetElementById(div.id + "_node");
        if(this.getController().isDragging()) {
            var ftNode = this._getFolderTreeNode(div);
            ftNode.onDragInTarget(e);
        } else {
            if(node === this._selectedNode) {
                return;
            }
            this._appendStyle(node, this._hoverClass, this._hoverLinkClass);
        }
    },
    UnhoverNode: function (div) {
        var node = WebForm_GetElementById(div.id + "_node");
        if(this.getController().isDragging()) {
            var ftNode = this._getFolderTreeNode(div);
            ftNode.onDragLeaveTarget();
        } else {
            if(node === this._selectedNode) {
                return;
            }
            this._restoreStyle(node);
        }
    },
    _getFolderTreeNode: function (div) {
        if(!div._folderTreeNode) {
            div._folderTreeNode = new FolderTreeNode(this, div);
        }
        return div._folderTreeNode;
    },
    _appendStyle: function (node, nodeClass, nodeLinkClass) {
        if(nodeClass) {
            if(!node._normalClass) {
                node._normalClass = node.className;
            }
            node.className = node._normalClass + " " + nodeClass;
        }
        if(nodeLinkClass) {
            var nodeLink = this._getNodeLink(node);
            if(!nodeLink) {
                return;
            }
            if(!nodeLink._normalClass) {
                nodeLink._normalClass = nodeLink.className;
            }
            nodeLink.className = nodeLink._normalClass + " " + nodeLinkClass;
        }
    },
    _restoreStyle: function (node) {
        if(node._normalClass) {
            node.className = node._normalClass;
        }
        var nodeLink = this._getNodeLink(node);
        if(nodeLink && nodeLink._normalClass) {
            nodeLink.className = nodeLink._normalClass;
        }
    }
};
var FolderTreeNode = function (owner, div) {
    this._owner = owner;
    this._div = div;
    this._node = WebForm_GetElementById(div.id + "_node");
    var mouseUp = this._mouseUp;
    var mouseMove = this._mouseMove;
    var instance = this;
    div.onmouseup = function (e) {
        e = e || window.event;
        mouseUp.call(instance, e);
    };
    div.onmousemove = function (e) {
        e = e || window.event;
        mouseMove.call(instance, e);
    };
};
FolderTreeNode.prototype._owner = null;
FolderTreeNode.prototype._div = null;
FolderTreeNode.prototype._dropMove = true;
FolderTreeNode.prototype._node = null;
FolderTreeNode.prototype._highlight = false;
FolderTreeNode.prototype._cursor = null;
FolderTreeNode.prototype.getFullPath = function () {
    var nodePath = "" + this._node.attributes["nodepath"].value;
    var index = nodePath.indexOf("|");
    return nodePath.substring(index + 1, nodePath.length);
};
FolderTreeNode.prototype.isSelected = function () {
    return this._node === this._owner._selectedNode;
};
FolderTreeNode.prototype.canDrop = function () {
    return !this.isSelected();
};
FolderTreeNode.prototype.getController = function () {
    return this._owner.getController();
};
FolderTreeNode.prototype.highlight = function (bool) {
    if(this._highlight == bool) {
        return;
    }
    this._highlight = bool;
    if(bool) {
        this._owner._appendStyle(this._node, this._owner._selectedClass, this._owner._selectedLinkClass);
    } else {
        this._owner._restoreStyle(this._node);
    }
};
FolderTreeNode.prototype.setCursor = function (cursor) {
    if(this._cursor == cursor) {
        return;
    }
    this._cursor = cursor;
    this._div.style.cursor = cursor;
    var nodeLink = this._owner._getNodeLink(this._node);
    if(nodeLink) {
        nodeLink.style.cursor = cursor;
    }
    var imgLink = this._owner._getImageLink(this._node);
    if(imgLink) {
        imgLink.style.cursor = cursor;
    }
};
FolderTreeNode.prototype._mouseUp = function (ev) {
    if(ev.preventDefault) {
        ev.preventDefault();
    }
    ev.returnValue = false;
    if(this.getController().isDragging()) {
        this.onDrop();
    }
    return false;
};
FolderTreeNode.prototype._mouseMove = function (ev) {
    if(ev.preventDefault) {
        ev.preventDefault();
    }
    ev.returnValue = false;
    if(this.getController().isDragging()) {
        this.onDragInTarget(ev);
    }
    return false;
};
FolderTreeNode.prototype.onDragInTarget = function (ev) {
    if(this.canDrop()) {
        this._dropMove = !ev.ctrlKey && !ev.shiftKey;
        this.getController()._dropTarget = this;
        this.highlight(true);
        if(this._dropMove) {
            this.setCursor(this.getController()._dropMoveCursor);
        } else {
            this.setCursor(this.getController()._dropCopyCursor);
        }
    } else {
        this.setCursor(this.getController()._dropNotAllowedCursor);
    }
};
FolderTreeNode.prototype.onDragLeaveTarget = function () {
    this.getController()._dropTarget = null;
    if(this.canDrop()) {
        this.highlight(false);
    }
    this.setCursor("");
};
FolderTreeNode.prototype.onDrop = function () {
    if(this.canDrop()) {
        this.getController().drop(this, this._dropMove);
        this.highlight(false);
    } else {
        this.getController().stopDragDrop(this);
    }
    this.setCursor("");
};
//@ sourceMappingURL=FolderTree.js.map
