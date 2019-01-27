chrome.storage.sync.get(['key'], function (result) {
    var x = new XMLHttpRequest();
    x.open('POST', 'http://localhost:51410/api/snapshot');
    x.setRequestHeader('Content-Type', 'application/json')
    x.onload = function() {
        if(x.status === 200) {
            swal({ title: 'Success', text: '', icon: 'success'}).then(result => {
                window.location.href = "http://localhost/AcumaticaERP/Main?ScreenId=AP3010PL";
            });
        }
    };
    x.send(JSON.stringify({
        'lessonId': result.key
    }));
});