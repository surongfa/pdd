var open1 = window.XMLHttpRequest.prototype.open,
    send1 = window.XMLHttpRequest.prototype.send,
    onReadyStateChange,
    cache_data=null;

function openReplacement(method, url, async, user, password) {
    var syncMode = async !== false ? 'async' : 'sync';
    return open1.apply(this, arguments);
}

function sendReplacement(data) {
    if (this.onreadystatechange) {
        this._onreadystatechange = this.onreadystatechange;
    }
    this.onreadystatechange = onReadyStateChangeReplacement;

    return send1.apply(this, arguments);
}

function onReadyStateChangeReplacement() {
	console.info(this.responseURL)
    var url=this.responseURL;
    var data=this.response;
    if(this.status == 200){
        window.dispatchEvent(new CustomEvent("hook", {
            detail:{url:url, data:data}
        }));
    }


    if (this._onreadystatechange) {
        return this._onreadystatechange.apply(this, arguments);
    }
}

let ajax_interceptor_qoweifjqon = {
  originalFetch: window.fetch.bind(window),
  myFetch: function(...args) {
    return ajax_interceptor_qoweifjqon.originalFetch(...args).then((response) => {
		//console.info(response.url);
      if(window.kcxg && (response.url.indexOf("vodka/v2/mms/query/display/mall/goodsList") > -1))
	  {
		  response.json().then(data => {  // 异步执行，返回不了data
			if(data && data.success)
			{
				window.total = data.result.total;
				window.dispatchEvent(new CustomEvent("hook", {
					detail:{url:response.url, data:data}
				}));
			}
		  });

		const stream = new ReadableStream({
          start(controller) {
            controller.enqueue(new TextEncoder().encode(JSON.stringify({"success":true,"errorCode":1000000,"errorMsg":null,"result":{"sessionId":"6edc7b4862514cb3a2c0a3da73fccf99","total":window.total??10000,"goods_list":[]}})));
            controller.close();
          }
        });
  
        const newResponse = new Response(stream, {
          headers: response.headers,
          status: response.status,
          statusText: response.statusText,
        });
        const proxy = new Proxy(newResponse, {
          get: function(target, name){
            switch(name) {
              case 'ok':
              case 'redirected':
              case 'type':
              case 'url':
              case 'useFinalURL':
              case 'body':
              case 'bodyUsed':
                return response[name];
            }
            return target[name];
          }
        });
  
        for (let key in proxy) {
          if (typeof proxy[key] === 'function') {
            proxy[key] = proxy[key].bind(newResponse);
          }
        }
		return proxy;
	  }else {
        return response;
      }

    });
  },
}
window.fetch = ajax_interceptor_qoweifjqon.myFetch;
window.XMLHttpRequest.prototype.open = openReplacement;
window.XMLHttpRequest.prototype.send = sendReplacement;

/**var cookiedata = document.createElement("div")
cookiedata.setAttribute('id', 'cookie-data');
cookiedata.setAttribute('style', 'display:none');
cookiedata.innerText = document.cookie;
document.documentElement.appendChild(cookiedata);
**/