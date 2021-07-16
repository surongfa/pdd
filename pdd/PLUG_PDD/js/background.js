window.caijistart = false;
chrome.extension.onMessage.addListener(function(request, _, sendResponse){
    switch (request.type) {
        case "debug":
            console.log(request);
            return true;
        case "get":
            $.ajax({
                type:
                    "GET",
                url: request.url,
                success: function(data) {
                    sendResponse({
                        status: true,
                        result: data
                    })
                },
                error: function(err) {
                    sendResponse({
                        status: false,
                        result: err
                    })
                }
            });

            return true;
        case "post":
            $.ajax({
                type:
                    "POST",
                url: request.url,
                data: request.data || {},
                dataType: request.dataType || "json",
                success: function(data) {
                    sendResponse({
                        status: true,
                        result: data
                    })
                },
                error: function(err) {
                    sendResponse({
                        status: false,
                        result: err
                    })
                }
            });
            return true;
        case "postjson":
			if(!window.caijistart)
				return false;
            $.ajax({
                type:
                    "POST",
                url: request.url,
                data: request.data || {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    sendResponse({
                        status: true,
                        result: data,
						caijistart:window.caijistart
                    })
                },
                error: function(err) {
                    sendResponse({
                        status: false,
                        result: err,
						caijistart:window.caijistart
                    })
                }
            });
            return true
		case "postcookie":
			if(!window.kcxg)
				return false;
			debugger
			if(request.data)
				request.data.cookie=cookies
            $.ajax({
                type:
                    "POST",
                url: request.url,
                data: request.data || {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    sendResponse({
                        status: true,
                        result: data,
						caijistart:window.caijistart
                    })
                },
                error: function(err) {
                    sendResponse({
                        status: false,
                        result: err,
						caijistart:window.caijistart
                    })
                }
            });
            return true;
		case "cookie":
			if(request.data)
				request.data.cookie=cookies
            $.ajax({
                type:
                    "POST",
                url: request.url,
                data: request.data || {},
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    sendResponse({
                        status: true,
                        result: data
                    })
                },
                error: function(err) {
                    sendResponse({
                        status: false,
                        result: err
                    })
                }
            });
            return true;
        case "getversion":
            var manifest = chrome.runtime.getManifest();
            sendResponse(manifest.version);
            return true;
		case "isstart":
			$.ajax({
				type:
					"POST",
				url: "http://127.0.0.1:8080/goods/set",
				data: request.data || {},
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function(data) {
					if(data && data.data)
					{
						window.caijicontent = data.data.caijicontent;
						window.autocaiji = data.data.autocaiji;
						window.caijisize = data.data.caijisize;
						window.caijinewgoods = data.data.caijinewgoods;
					}
				}
			});
            sendResponse({caijistart:window.caijistart, kcxg:window.kcxg, caijicontent:window.caijicontent, caijisize:window.caijisize, autocaiji:window.autocaiji, caijinewgoods:window.caijinewgoods});
            return true;
		case "tip":
			if(window.caijistart && request.url =="run")
			{
				chrome.browserAction.setBadgeText({text: 'run'});
				chrome.browserAction.setBadgeBackgroundColor({color: [0, 0, 204, 255]});
			}else if(request.url =="ex")
			{
				chrome.browserAction.setBadgeText({text: 'ex'});
				chrome.browserAction.setBadgeBackgroundColor({color: [255, 0, 0, 255]});
			}else if(!window.caijistart || request.url =="stop")
			{
				chrome.browserAction.setBadgeText({text: 'stop'});
				chrome.browserAction.setBadgeBackgroundColor({color:"black"});
			}
            
            return true;
		case "caijistart":
			if(request.url =="start")
			{
				window.caijistart = true;
				$.ajax({
					type:
						"POST",
					url: "http://127.0.0.1:8080/goods/set",
					data: request.data || {},
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: function(data) {
						if(data && data.data)
						{
							window.caijicontent = data.data.caijicontent;
							window.autocaiji = data.data.autocaiji;
							window.caijisize = data.data.caijisize;
							window.caijinewgoods = data.data.caijinewgoods;
						}
					}
				});
				chrome.browserAction.setBadgeText({text: 'run'});
				chrome.browserAction.setBadgeBackgroundColor({color: [0, 0, 204, 255]});
			}else if(request.url =="stop")
			{
				window.caijistart = false;
				window.caijicontent = "";
				window.autocaiji = false;
				window.caijisize = 0;
				window.caijinewgoods  =0;
				chrome.browserAction.setBadgeText({text: 'stop'});
				chrome.browserAction.setBadgeBackgroundColor({color: "black"});
			}
            if(request.url =="startkc")
			{
				window.kcxg = true;
				//chrome.browserAction.setBadgeText({text: 'run'});
				//chrome.browserAction.setBadgeBackgroundColor({color: [0, 0, 204, 255]});
			}else if(request.url =="stopkc")
			{
				window.kcxg = false;
				//chrome.browserAction.setBadgeText({text: 'stop'});
				//chrome.browserAction.setBadgeBackgroundColor({color: "black"});
			}
            return true;
		case "cookies":
            sendResponse({cookies:cookies});
            return true;
    }

});

var curDomain = null;
var cookies = null;
//域名正则匹配
var regStr = '(\\w*:\/\/)?((\\w*\\-)*\\w*\.(com.cn|com|net.cn|net|org.cn|name|org|gov.cn|gov|cn|com.hk|mobi|me|info|name|biz|cc|tv|asia|hk|网络|公司|中国|ac.cn|bj.cn|sh.cn|tj.cn|cq.cn|he.cn|sx.cn|nm.cn|ln.cn|jl.cn|hl.cn|js.cn|zj.cn|ah.cn|fj.cn|jx.cn|sd.cn|ha.cn|hb.cn|hn.cn|gd.cn|gx.cn|hi.cn|sc.cn|gz.cn|yn.cn|xz.cn|sn.cn|gs.cn|qh.cn|nx.cn|xj.cn|tw.cn|hk.cn|mo.cn|travel|tw|com.tw|sh|ac|io|ws|us|tm|vc|ag|bz|in|mn|me|sc|co|org.tw|jobs|tel))';
var regx = new RegExp(regStr,'gi');

chrome.tabs.onUpdated.addListener(function (tabId, changeInfo, tab) {
	if (changeInfo.status == 'complete' && tab.url.indexOf("mms.pinduoduo.com")>-1) {
		var result = tab.url.match(regx);
		curDomain = result[0].replace("/","");

        if (!chrome.cookies) {
            chrome.cookies = chrome.experimental.cookies;
        }
		var cookies1 = "";
        chrome.cookies.getAll({},function (cookie) {
			var su=[];
            for (i = 0; i < cookie.length; i++) {
                var domainResult = cookie[i].domain.match(regx);
                if (domainResult != null && domainResult.length > 0 && curDomain.toLowerCase() == domainResult[0].replace("/", "").toLowerCase()) {
					if(su.indexOf(cookie[i].name + "=" + cookie[i].value + "; ")==-1){
						cookies1 += cookie[i].name + "=" + cookie[i].value + "; ";
					}
                }
            }
            if (cookies1 != "") {
                cookies1 = cookies1.substr(0, cookies1.length - 2);
            }
			cookies = cookies1;
        });
	}
});

/**chrome.notifications.create(null, {
	type: 'basic',
	iconUrl: 'icons/icon128.png',
	title: '这是标题',
	message: '您刚才点击了自定义右键菜单！'
});

**/