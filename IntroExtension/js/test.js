setTimeout(function () {
    debugger;
    console.log('inject intro references')
    var iframe = window.frames["main"].document;
    var iFrameHead = iframe.getElementsByTagName("head")[0];
    var mycssscript = document.createElement('link');
    mycssscript.rel = 'stylesheet';
    mycssscript.type = 'text/css';
    mycssscript.href = '//cdnjs.cloudflare.com/ajax/libs/intro.js/2.9.3/introjs.min.css';
    iFrameHead.appendChild(mycssscript);

    var iFrameBody = iframe.getElementsByTagName("body")[0];
    var myscript = document.createElement('script');
    myscript.type = 'text/javascript';
    myscript.src = '//cdnjs.cloudflare.com/ajax/libs/intro.js/2.9.3/intro.min.js';
    iFrameBody.appendChild(myscript);

    iframe.getElementById('ctl00_phF_form_t0_edRefNbr_text').setAttribute('data-intro', 'Hello step one!');

    console.log('attr: ' + iframe.getElementById('ctl00_phF_form_t0_edRefNbr_text').getAttribute('data-intro'));


    var yourCustomJavaScriptCode = 'setTimeout(function () {introJs().start();}, 1000);';
    var script = document.createElement('script');
    var code = document.createTextNode('(function() {' + yourCustomJavaScriptCode + '})();');
    script.appendChild(code);
    iFrameBody.appendChild(script);

}, 3000);


