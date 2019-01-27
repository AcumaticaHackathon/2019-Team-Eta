window.addEventListener('load', function load(event) {
    document.getElementById('loginBtn').onclick = function() {
        chrome.tabs.executeScript({
            file: 'js/validation.js'
          }); 
    };
});


TreeView();

function TreeView() {
    window.addEventListener('load', function load(event) {
        var toggler = document.getElementsByClassName("caret");
        var i;

        for (i = 0; i < toggler.length; i++) {
            toggler[i].addEventListener("click", function () {
                this.parentElement.querySelector(".nested").classList.toggle("active");
                this.classList.toggle("caret-down");
            });
        }

        var steps = document.getElementsByClassName("step");
        var i;

        for (i = 0; i < steps.length; i++) {
            steps[i].addEventListener("click", function () {
                window.location.href = this.href;
            });
        }
    });
}