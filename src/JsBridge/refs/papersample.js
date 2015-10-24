function render() {
    with (paper) {
        // Create an empty project and a view for the canvas:
        setup(document.createElement("canvas"));

        var path = new Path.Rectangle([75, 75], [100, 100]);
        path.strokeColor = 'black';

        path.rotate(13);
        view.draw();

        //view.onFrame = function (event) {
        //    console.log('onFrame');
        //    // On each frame, rotate the path by 3 degrees:
        //    path.rotate(3);
        //}
    }
}
