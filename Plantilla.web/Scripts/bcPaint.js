(function ($) {
	var path;
	templates = {
		canvas: $('<canvas id="sheet" width="400" height="400></canvas>')
	},


	$.fn.bcPaint = function (options) {

		return this.each(function () {
			var rootElement = $(this),
				canvasPane = templates.canvas.clone();

			// assembly pane
			rootElement.append(canvasPane);

			// get canvas pane context
			paintCanvas = document.getElementById('sheet');

			// bind mouse actions
			paintCanvas.onmousedown = $.fn.bcPaint.onMouseDown;
			paintCanvas.onmouseup = $.fn.bcPaint.onMouseUp;
			paintCanvas.onmousedrag = $.fn.bcPaint.onMouseDrag;

		});
	}

	/**
	* Extend plugin
	**/
	$.extend(true, $.fn.bcPaint, {

		/**
		* Remove pane
		*/
		clearCanvas: function () {
			paintCanvas.width = paintCanvas.width;
		},

		/**
		* On mouse down
		**/
		onMouseDown: function (event) {
			// If we produced a path before, deselect it:
			if (path) {
				path.selected = false;
			}

			// Create a new path and set its stroke color to black:
			path = new Path({
				segments: [event.point],
				strokeColor: 'black'
			});
		},

		/**
		* On mouse up
		**/
		onMouseUp: function (event) {
			path.simplify(5);
		},

		/**
		* On mouse move
		**/
		onMouseDrag: function (event) {
			path.add(event.point);
		},
		export: function () {
			var imgData = paintCanvas.toDataURL('image/png');
			var windowOpen = window.open('about:blank', 'Image');
			windowOpen.document.write('<img src="' + imgData + '" alt="Exported Image"/>');
		},

		/**
		* Convert color to HEX value
		**/

	});


})(jQuery);