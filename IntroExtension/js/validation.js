chrome.storage.sync.get(['key'], function (result) {

    var x = new XMLHttpRequest();
    x.open('POST', 'http://localhost:51410/api/validation');
    x.setRequestHeader('Content-Type', 'application/json')
    x.onload = function() {
        if(x.status === 200) {
            swal({ title: 'Success', text: '', icon: 'success'});
        }

        if(x.status === 400) {
            swal({ title: '', text: x.responseText, icon: 'error'});
        }
    };
    x.send(JSON.stringify({
        'lessonId': result.key
    }));
});