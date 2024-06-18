document.addEventListener("DOMContentLoaded", function () {
    var canvas = document.getElementById('myCanvas');
    if (canvas.getContext) {
        var ctx = canvas.getContext('2d');

        // Draw a simple rectangle
        ctx.fillStyle = 'rgb(200, 0, 0)';
        ctx.fillRect(10, 10, 50, 50);

        // Draw a simple circle
        ctx.beginPath();
        ctx.arc(150, 75, 50, 0, Math.PI * 2, true);
        ctx.fillStyle = 'rgb(0, 0, 200)';
        ctx.fill();
    }
});