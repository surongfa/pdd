var start = document.getElementById("caiji");
chrome.runtime.sendMessage({type: 'isstart',url:"start"}, function(response) {
	if(response.caijistart)
		start.innerText="停止采集";
	else
		start.innerText="启动采集";
});
	
caiji.onclick=function(){
	this.disabled = true
	if(this.innerText == "启动采集"){
		this.innerText = "停止采集"
		chrome.runtime.sendMessage({type: 'caijistart',url:"start"});
	}else{
		this.innerText = "启动采集"
		chrome.runtime.sendMessage({type: 'caijistart',url:"stop"});
	}
	this.disabled = false
}

/**var stop = document.getElementById("stop");
stop.onclick=function(){
	console.log("stop");
chrome.runtime.sendMessage({type: 'caijistart',url:"stop"});
}


	var startkc = document.getElementById("startkc");
startkc.onclick=function(){
	console.log("startkc");
		chrome.runtime.sendMessage({type: 'caijistart',url:"startkc"});
}

var stopkc = document.getElementById("stopkc");
stopkc.onclick=function(){
	console.log("stopkc");
chrome.runtime.sendMessage({type: 'caijistart',url:"stopkc"});
}**/