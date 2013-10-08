/// <reference path="WebForm.d.ts" />
var FileManagerController = function (ClientID, UniqueID, EventArgumentSplitter) {
    this.ClientID = ClientID;
    this.UniqueID = UniqueID;
    this.EventArgumentSplitter = EventArgumentSplitter;
    this.documentOnClick = new Array();
    this.TextBox = document.getElementById(this.ClientID + '_TextBox');
    this.TextBox.HideFunction = 'WFM_' + ClientID + '.HideTextBox()';
    this.doNotHide = false;
    this.doRename = false;

    if (document.onclick) {
        this.documentOnClick[this.documentOnClick.length] = document.onclick;
        this.documentOnClick[this.documentOnClick.length] = function (e) {
            eval('WFM_' + ClientID + '.HideTextBox()');
        };
        document.onclick = function (e) {
            eval('WFM_' + ClientID + '.evalDocumentOnClick(e)');
        };
    } else {
        document.onclick = function (e) {
            eval('WFM_' + ClientID + '.HideTextBox()');
        };
    }

    var mouseUp = this._onMouseUp;
    var keyPress = this._onKeyPress;
    var instance = this;
    this._dnd_document_onmouseup = function (e) {
        e = e || window.event;
        mouseUp.call(instance, e);
    };
    this._dnd_document_onkeypress = function (e) {
        e = e || window.event;
        keyPress.call(instance, e);
    };
};

FileManagerController.prototype.evalDocumentOnClick = function (e) {
    for (var i = 0; i < this.documentOnClick.length; i++) {
        this.documentOnClick[i](e);
    }
};

FileManagerController.prototype.HideTextBox = function () {
    if (this.doNotHide) {
        this.doNotHide = false;
        return;
    }
    this.TextBox.style.display = "none";
    this.TextBox.style.visibility = "hidden";
    if (this.doRename) {
        if ('' + this.TextBox.Item.Name != this.TextBox.value) {
            if (this.TextBox.Control.InProcess)
                return;
            this.TextBox.Control.ShowProgress();
            WebFileManager_InitCallback();
            WebFileManager_DoCallback(this.UniqueID, this.TextBox.Control.ClientID + this.EventArgumentSplitter + 'Rename' + this.EventArgumentSplitter + this.TextBox.Item.Path + this.EventArgumentSplitter + encodeURIComponent(this.TextBox.value), WebFileManager_Eval, this.TextBox.Control, WebFileManager_OnError);
        } else {
            this.TextBox.Control.SetFocus();
        }
    }
    this.doRename = false;
};

FileManagerController.prototype.OnRename = function (sender, arg) {
    if (sender.InProcess)
        return;
    if (sender.SelectedItems.length == 0)
        return;
    var item = null;
    for (var i = 0; i < sender.SelectedItems.length; i++) {
        if (sender.SelectedItems[i].CanBeRenamed) {
            item = sender.SelectedItems[i];
            break;
        }
    }
    if (!item)
        return;
    var name = document.getElementById(item.id + '_Name');
    var pos = WebForm_GetElementPosition(name);
    var ClientID = this.ClientID;

    this.doNotHide = !this.doNotHide;
    this.doRename = true;

    this.TextBox.value = item.Name;
    this.TextBox.className = sender.EditTextBoxStyle;
    this.TextBox.style.display = "inline";
    this.TextBox.style.visibility = "visible";
    this.TextBox.style.top = '' + (pos.y - sender.Element.scrollTop) + 'px';
    this.TextBox.style.left = '' + (pos.x - sender.Element.scrollLeft) + 'px';
    this.TextBox.style.width = '' + pos.width + 'px';
    this.TextBox.style.height = '' + pos.height + 'px';
    this.TextBox.focus();
    this.TextBox.select();
    this.TextBox.Item = item;
    this.TextBox.Control = sender;

    this.TextBox.onclick = function (e) {
        if (e == null)
            var e = event;
        e.cancelBubble = true;
    };

    this.TextBox.onkeydown = function (e) {
        if (e == null)
            var e = event;
        if (e.keyCode == 27) {
            eval('WFM_' + ClientID + '.doRename = false');
            eval(this.HideFunction);
            this.Control.SetFocus();
            e.cancelBubble = true;
            return false;
        }
        if (e.keyCode == 13) {
            eval(this.HideFunction);
            e.cancelBubble = true;
            return false;
        }
    };
};

FileManagerController.prototype.OnRefresh = function (sender, arg) {
    if (sender.InProcess)
        return;
    sender.ShowProgress();
    WebFileManager_InitCallback();
    WebFileManager_DoCallback(this.UniqueID, sender.ClientID + this.EventArgumentSplitter + 'Refresh' + this.EventArgumentSplitter + arg, WebFileManager_Render, sender, WebFileManager_OnError);
};

FileManagerController.prototype.OnExecuteCommand = function (sender, arg) {
    if (sender.InProcess)
        return;
    sender.ShowProgress();
    WebFileManager_InitCallback();
    WebFileManager_DoCallback(this.UniqueID, sender.ClientID + this.EventArgumentSplitter + 'ExecuteCommand' + this.EventArgumentSplitter + arg, WebFileManager_Eval, sender, WebFileManager_OnError);
};

FileManagerController.prototype.OnSelectedItemsDelete = function (sender, arg) {
    if (sender.InProcess)
        return;
    if (sender.SelectedItems.length > 0 && confirm(decodeURIComponent(eval('WFM_' + this.ClientID + 'DeleteConfirm')))) {
        sender.ShowProgress();
        WebFileManager_InitCallback();
        WebFileManager_DoCallback(this.UniqueID, sender.ClientID + this.EventArgumentSplitter + 'SelectedItemsDelete' + this.EventArgumentSplitter + arg, WebFileManager_Eval, sender, WebFileManager_OnError);
    }
};

FileManagerController.prototype.OnSelectedItemsCopyTo = function (sender, arg) {
    var _this = this;
    if (sender.InProcess)
        return;
    if (sender.SelectedItems.length == 0)
        return;
    var directory = decodeURIComponent(sender.GetDirectory());
    this.PromptDirectory(directory, function (arg) {
        return _this._SelectedItemsCopyTo(sender, arg);
    });
};

FileManagerController.prototype._SelectedItemsCopyTo = function (sender, arg) {
    if (arg) {
        sender.ShowProgress();
        WebFileManager_InitCallback();
        WebFileManager_DoCallback(this.UniqueID, sender.ClientID + this.EventArgumentSplitter + 'SelectedItemsCopyTo' + this.EventArgumentSplitter + encodeURIComponent(arg), WebFileManager_Eval, sender, WebFileManager_OnError);
    }
};

FileManagerController.prototype.OnSelectedItemsMoveTo = function (sender, arg) {
    var _this = this;
    if (sender.InProcess)
        return;
    if (sender.SelectedItems.length == 0)
        return;
    var directory = decodeURIComponent(sender.GetDirectory());
    this.PromptDirectory(directory, function (arg) {
        return _this._SelectedItemsMoveTo(sender, arg);
    });
};

FileManagerController.prototype._SelectedItemsMoveTo = function (sender, arg) {
    if (arg) {
        sender.ShowProgress();
        WebFileManager_InitCallback();
        WebFileManager_DoCallback(this.UniqueID, sender.ClientID + this.EventArgumentSplitter + 'SelectedItemsMoveTo' + this.EventArgumentSplitter + encodeURIComponent(arg), WebFileManager_Eval, sender, WebFileManager_OnError);
    }
};

FileManagerController.prototype.PromptDirectory = function (directory, callback) {
    var _this = this;
    var func = window['WFM_' + this.ClientID + 'PromptDirectory'] || function (dir, cb) {
        var selectedFolder = window.prompt(decodeURIComponent(eval('WFM_' + _this.ClientID + 'SelectDestination')), dir);
        cb(selectedFolder);
    };

    func(directory, callback);
};

FileManagerController.prototype.OnNewFolder = function (sender, arg) {
    if (sender.InProcess)
        return;
    sender.ShowProgress();
    WebFileManager_InitCallback();
    WebFileManager_DoCallback(this.UniqueID, sender.ClientID + this.EventArgumentSplitter + 'NewFolder' + this.EventArgumentSplitter + arg, WebFileManager_Eval, sender, WebFileManager_OnError);
};

FileManagerController.prototype.OnNewDocument = function (sender, arg) {
    if (sender.InProcess)
        return;
    sender.ShowProgress();
    WebFileManager_InitCallback();
    WebFileManager_DoCallback(this.UniqueID, sender.ClientID + this.EventArgumentSplitter + 'NewDocument' + this.EventArgumentSplitter + arg, WebFileManager_Eval, sender, WebFileManager_OnError);
};

FileManagerController.prototype.OnFileViewChangeView = function (sender, arg) {
    if (sender.InProcess)
        return;
    sender.SetView(arg);
    this.OnRefresh(sender, '');
};

FileManagerController.prototype.OnFileViewSort = function (sender, arg) {
    if (sender.InProcess)
        return;
    sender.SetSort(arg);
    this.OnRefresh(sender, '');
};

FileManagerController.prototype.OnFileViewNavigate = function (sender, arg) {
    if (sender.InProcess)
        return;
    sender.ShowProgress();
    WebFileManager_InitCallback();
    WebFileManager_DoCallback(this.UniqueID, sender.ClientID + this.EventArgumentSplitter + 'FileViewNavigate' + this.EventArgumentSplitter + encodeURIComponent(arg), WebFileManager_Eval, sender, WebFileManager_OnError);
};

FileManagerController.prototype.OnSearch = function (sender, arg) {
    if (sender.InProcess)
        return;
    var search = WebForm_GetElementById(sender.ClientID + '_SearchTerm');
    if (search)
        search.value = arg;
    if (arg)
        sender.SetView('Details');
    this.OnRefresh(sender, '');
};

FileManagerController.prototype.getDndVisual = function () {
    if (this._dndVisual == null) {
        this._dndVisual = document.createElement('div');
        this._dndVisual.style.visibility = "hidden";
        this._dndVisual.style.display = "none";
        document.body.appendChild(this._dndVisual);
    }
    return this._dndVisual;
};

FileManagerController.prototype._dropNotAllowedCursor = "not-allowed";
FileManagerController.prototype._dropCopyCursor = 'url("<% = WebResource("IZ.WebFileManager.resources.drag_copy.cur") %>"), default';
FileManagerController.prototype._dropMoveCursor = 'url("<% = WebResource("IZ.WebFileManager.resources.drag_move.cur") %>"), default';
FileManagerController.prototype._isDragging = false;
FileManagerController.prototype._dragSource = null;

FileManagerController.prototype.startDragDrop = function (dragSource) {
    this._wireEvents();
    this._isDragging = true;
    this._dragSource = dragSource;
};

FileManagerController.prototype.stopDragDrop = function () {
    this._unwireEvents();
    this._isDragging = false;
    this._dragSource = null;
    if (this._dropTarget) {
        this._dropTarget.onDragLeaveTarget();
        this._dropTarget = null;
    }
};

FileManagerController.prototype.isDragging = function () {
    return this._isDragging;
};

FileManagerController.prototype.drop = function (target, move) {
    var dragSource = this._dragSource;
    this.stopDragDrop();
    if (move)
        this._SelectedItemsMoveTo(dragSource, target.getFullPath());
else
        this._SelectedItemsCopyTo(dragSource, target.getFullPath());
};

FileManagerController.prototype._wireEvents = function () {
    this._origin_document_onmouseup = document.onmouseup;
    this._origin_document_onkeypress = document.onkeypress;
    document.onmouseup = this._dnd_document_onmouseup;
    document.onkeypress = this._dnd_document_onkeypress;
};

FileManagerController.prototype._unwireEvents = function () {
    document.onmouseup = this._origin_document_onmouseup;
    document.onkeypress = this._origin_document_onkeypress;
};

FileManagerController.prototype._onMouseUp = function (e) {
    this.stopDragDrop();
};

FileManagerController.prototype._onKeyPress = function (e) {
    // Escape.
    var k = e.keyCode ? e.keyCode : e.rawEvent.keyCode;
    if (k == 27) {
        this.stopDragDrop();
    }
};

function WebFileManager_InitCallback() {
    __theFormPostData = "";
    __theFormPostCollection = new Array();
    WebForm_InitCallback();
}

function WebFileManager_Render(result, context) {
    var interval = window.setInterval(function () {
        window.clearInterval(interval);
        context.Element.innerHTML = result;
        context.Initialize();
        context.SetFocus();
        context.HidePrgress();
    }, 0);
}

function WebFileManager_Eval(result, context) {
    //alert(result);
    var interval = window.setInterval(function () {
        window.clearInterval(interval);
        context.HidePrgress();
        eval(result);
    }, 0);
}

function WebFileManager_OnError(result, context) {
    var interval = window.setInterval(function () {
        window.clearInterval(interval);
        var parts = result.split("|", 2);
        if (parts.length > 1) {
            result = result.substr(0, parts[0].length - (parts[1].length + '').length);
        }
        context.SetFocus();
        context.HidePrgress();
        var div = document.createElement('DIV');
        div.innerHTML = result;
        alert(div.textContent || div.innerText);
    }, 0);
}

(function () {
    var isAChildOf = function (_parent, _child) {
        if (_parent === _child) {
            return false;
        }
        while (_child && _child !== _parent) {
            _child = _child.parentNode;
        }

        return _child === _parent;
    };

    var mouseHandler = function (el, event, fn) {
        var relTarget = event.relatedTarget;
        if (el === relTarget || isAChildOf(el, relTarget)) {
            return;
        }
        fn();
    };

    IZWebFileManager_MouseHover = function (el, event, id) {
        event = event || window.event;
        var fn = function () {
            IZWebFileManager_ShowElement(id);
        };
        mouseHandler(el, event, fn);
    };

    IZWebFileManager_MouseOut = function (el, event, id) {
        event = event || window.event;
        var fn = function () {
            IZWebFileManager_HideElement(id);
        };
        mouseHandler(el, event, fn);
    };

    IZWebFileManager_ShowElement = function (id) {
        var el = WebForm_GetElementById(id);
        el.style.display = 'block';
    };

    IZWebFileManager_HideElement = function (id) {
        var el = WebForm_GetElementById(id);
        el.style.display = 'none';
    };
})();
