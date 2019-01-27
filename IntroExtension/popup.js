window.addEventListener('load', function load(event) {
    document.getElementById('loginBtn').onclick = function () {
        chrome.tabs.executeScript({
            file: 'js/validation.js'
        });
    };

    var substeps = document.getElementsByClassName('substep');
    for (i = 0; i < substeps.length; i++) {
        substeps[i].onclick = function () {
            chrome.tabs.executeScript({
                file: 'js/redirect.js'
            });
        };
    }
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

        var steps = document.getElementsByClassName("substep");
        var i;

        for (i = 0; i < steps.length; i++) {
            steps[i].addEventListener("click", function () {
                chrome.storage.sync.set({ key: this.text }, function () {
                });
            });
        }
    });
}