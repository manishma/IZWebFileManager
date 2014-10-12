/// <reference path="../../../Scripts/jquery-1.11.0.js" />
/// <reference path="../../../Scripts/jquery-1.11.0.intellisense.js" />
/*******************************************
	MbWaitSpinner
	A simple spin.js implementation to 
	wondow load event
	(c) Bruno Milgiaretti 2014
********************************************/
;var MbWaitSpinner;

(function () {
	"use strict";
	
	MbWaitSpinner = function (opt) {
		if (!$().spin ) throw (new Error('Spin.js required !!!'));
		var defaults = {
			locked: false,
			overlayColor: "transparent",
			color: "#000",
            callback: null
		};
		var options = {};
		$.extend(options, defaults, opt);
		this.locked = options.locked;
		this.overlayColor = options.windowColor;
		this.spinOptions = options;
		this.callback = options.callback;
	}
	MbWaitSpinner.prototype = {
		show: function () {
			this.$element = $('<div />');
			if (this.locked) {
				this.$element
					.css({
						position: "fixed",
						top: 0,
						bottom: 0,
						left: 0,
						right: 0,
						"background-color": this.overlayColor,
                        "z-index": 1000
					});
			} else {
				this.$element
					.css({
						position: "fixed",
						top:"50%",
						left:"50%",
						height:"80px",
						width: "80px",
						margin: "-40px 0 0 -40px"
					});
			}
			this.$element.appendTo("body").spin(this.spinOptions);
		},
		hide: function () {
			this.$element.remove();
		}
	};
	
	$.fn.mb_waitspinner = function(options) {
		return $(this).each(function(index, element) {
			var ws;
			if(index == 0) {
				ws = new MbWaitSpinner(options);
				ws.show();
				$(window).load(function () {
				    if ($.isFunction(ws.callback)) {
				        if (ws.callback(ws))
					        ws.hide();
				    }
				    else
				    {
				        ws.hide();
				    }
				})
			}
		});
	}
	
}).call(this);