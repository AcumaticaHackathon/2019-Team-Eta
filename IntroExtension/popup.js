window.addEventListener('load', function load(event) {
    document.getElementById('backBtn').onclick = function () {
        chrome.storage.sync.remove('key');
        reloadScreen();
    };

    document.getElementById('loginBtn').onclick = function () {
        hideElement('login_data');
        showElement('tree');

        chrome.storage.sync.set({ key: '6.1.3' });
    };

    document.getElementById('helpBtn').onclick = function () {
        chrome.tabs.executeScript({
            file: 'js/wizard.js'
        });
    };

    document.getElementById('validateBtn').onclick = function () {
        chrome.tabs.executeScript({
            file: 'js/validation.js'
        });
    };

    reloadScreen();
});

TreeView();

function reloadScreen() {
    chrome.storage.sync.get(null, function (items) {
        var allKeys = Object.keys(items);
        if (!allKeys.includes('key')) {
            hideElement('tree');
            hideElement('activeClass');
            showElement('login_data');
        }
        else {
            hideElement('login_data');
            hideElement('tree');
            showElement('activeClass');
        }
    });
}

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

function showElement(elementId) {
    var x = document.getElementById(elementId);
    x.style.display = "block";
}

function hideElement(elementId) {
    var x = document.getElementById(elementId);
    x.style.display = "none";
}