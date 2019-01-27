window.addEventListener('load', function load(event) {
    document.getElementById('loginBtn').onclick = function() {
        chrome.tabs.executeScript({
            file: 'js/validation.js'
          }); 
    };
});