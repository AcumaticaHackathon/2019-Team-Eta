chrome.storage.sync.get(['key'], function (result) {

/*    swal({
        title: "Good job!",
        text: "You clicked the button!",
        icon: "success",
    });*/

    var x = new XMLHttpRequest();
    x.open('POST', 'http://localhost:51410/api/validation');
    x.setRequestHeader('Content-Type', 'application/json')
    x.onload = function() {
        if(x.status === 200) {
            alert('success');
            //todo laura replace :)
        }
    };
    x.send(JSON.stringify({
        'lessonId': result.key
    }));
});