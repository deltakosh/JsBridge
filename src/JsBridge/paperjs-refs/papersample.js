paper.setup(document.createElement("canvas"));

// Create an empty project and a view for the canvas:
var path = new paper.Path.Rectangle([75, 75], [100, 100]);
path.strokeColor = 'black';

function drawScene() {
    path.rotate(3);

    paper.view.draw();
}
