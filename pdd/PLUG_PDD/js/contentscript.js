
//let host='http://demo.com/';
let host='http://127.0.0.1:8080/';
let api_url_goods_add=host+'goods/add';
let api_url_details_get=host+'goods/get_details';
let api_url_details_set=host+'goods/set_details';
let api_url_sj_stock=host+'goods/stock';

chrome.runtime.sendMessage({type: 'tip',url:"run"});
const debug=(data)=>{
    chrome.runtime.sendMessage({type: 'debug',data:data}, function(response) {
        console.log("send_msg:"+response);
    });
};
const log=(d)=>{
	
	if(d=="采集停止"){
		chrome.runtime.sendMessage({type: 'tip',url:"stop"});
	}else if(d=="网络异常"){
		chrome.runtime.sendMessage({type: 'tip',url:"ex"});
	}
    //console.log("PDD采集==》"+d);
};
const sleep=(time)=> {
  return new Promise((resolve) => setTimeout(resolve, time));
}
const get_caijistart=()=>{
	chrome.runtime.sendMessage({type: 'isstart'}, function(response) {
		window.caijistart = !!response.caijistart;
		window.caijicontent = response.caijicontent;
		window.caijisize = response.caijisize;
		window.autocaiji = response.autocaiji;
		window.caijinewgoods = response.caijinewgoods;
		log("拼多多商城采集是否启动："+window.caijistart+","+window.caijicontent+","+window.caijisize);
	 });
		
};

const getQueryVariable=(variable)=>{
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i=0;i<vars.length;i++) {
        var pair = vars[i].split("=");
        if(pair[0] == variable){return pair[1];}
    }
    return(false);
};
const load_good_list=(data)=>{
    try {
        var obj = JSON.parse(data);
        var list = obj.goods_list;
        var goods=[];
        for(var i=0;i<list.length;i++){
            var item={
                'goods_id':list[i].goods_id,
                'goods_name':list[i].goods_name,
                'image_url':list[i].thumb_url,
                'price_info':list[i].price / 100,
                'sales':list[i].sales,
				'newgoods':list[i].tag_list && JSON.stringify(list[i].tag_list).indexOf("新品")>-1?1:0,
            };
            goods.push(item);
        }
        log("采集商品数量："+goods.length);
        chrome.runtime.sendMessage({type: 'postjson',url:api_url_goods_add,data:JSON.stringify(goods)}, function(response) {
			if(response == undefined)
			{
				log("采集停止");
			}else if(response.status){
                if(response.result.code == 1){
                    log(response.result.msg);
                }else{
                    log(response.result.msg);
                }
            }else{
                log("网络异常");
            }
        });

    }catch (e) {

    }
};

//首页分类
const load_good_list_index_cate=(data)=>{
    try {
        var obj = JSON.parse(data);
        var list = obj.list;
        var goods=[];
        for(var i=0;i<list.length;i++){
            var item={
                'goods_id':list[i].goods_id,
                'goods_name':list[i].goods_name,
                'image_url':list[i].thumb_url,
                'price_info':list[i].price / 100,
                'sales':list[i].sales,
				'newgoods':list[i].tag_list && JSON.stringify(list[i].tag_list).indexOf("新品")>-1?1:0,
            };
            goods.push(item);
        }
        log("采集商品数量："+goods.length);
        chrome.runtime.sendMessage({type: 'postjson',url:api_url_goods_add,data:JSON.stringify(goods)}, function(response) {
            if(response == undefined)
			{
				log("采集停止");
			}else if(response.status){
                if(response.result.code == 1){
                    log(response.result.msg);
                }else{
                    log(response.result.msg);
                }
            }else{
                log("网络异常");
            }
        });

    }catch (e) {

    }
}
const load_good_list_search_result=(data, source)=>{
    try {
        var obj = JSON.parse(data);
		var list = [];
		if(source=="catgoods"){
			for(let item in obj.store.tabListMap){
				list = obj.store.tabListMap[item].tabGoodsList.list;
			}
		}else{
			list = obj.store.data.ssrListData.list;
		}
        var goods=[];
        for(var i=0;i<list.length;i++){
			var sales = 0;
			if(list[i].salesTip.indexOf("+")>-1){
				sales=100000;
			}else if(list[i].salesTip.indexOf("万")>-1){
				sales=list[i].salesTip.replace("已拼","").replace("件","").replace("万","") * 10000;
			}else{
				sales=list[i].salesTip.replace("已拼","").replace("件","");
			}
			
            var item={
                'goods_id':list[i].goodsID,
                'goods_name':list[i].goodsName,
                'image_url':list[i].imgUrl,
                'price_info':list[i].price / 100,
                'sales':sales,
				'newgoods':list[i].tagList && JSON.stringify(list[i].tagList).indexOf("新品")>-1?1:0,
            };
            goods.push(item);
        }
        log("采集商品数量："+goods.length);
        chrome.runtime.sendMessage({type: 'postjson',url:api_url_goods_add,data:JSON.stringify(goods)}, function(response) {
            if(response == undefined)
			{
				log("采集停止");
			}else if(response.status){
                if(response.result.code == 1){
                    log(response.result.msg);
                }else{
                    log(response.result.msg);
                }
            }else{
                log("网络异常");
            }
        });

    }catch (e) {
log(e)
    }
}
const load_good_list_search=(data)=>{
    try {
        var obj = JSON.parse(data);
        var list = obj.items;
        var goods=[];
        for(var i=0;i<list.length;i++){
            var item={
                'goods_id':list[i].item_data.goods_model.goods_id,
                'goods_name':list[i].item_data.goods_model.goods_name,
                'image_url':list[i].item_data.goods_model.thumb_url,
                'price_info':list[i].item_data.goods_model.price / 100,
                'sales':list[i].item_data.goods_model.sales,
				'newgoods':list[i].item_data.goods_model.tag_list && JSON.stringify(list[i].item_data.goods_model.tag_list).indexOf("新品")>-1 ? 1:0,
            };
            goods.push(item);
        }
        log("采集商品数量："+goods.length);
        chrome.runtime.sendMessage({type: 'postjson',url:api_url_goods_add,data:JSON.stringify(goods)}, function(response) {
            if(response == undefined)
			{
				log("采集停止");
			}else if(response.status){
                if(response.result.code == 1){
                    log(response.result.msg);
                }else{
                    log(response.result.msg);
                }
            }else{
                log("网络异常");
            }
        });

    }catch (e) {

    }
}
const load_sj_good_list=(data)=>{
    try {
		var a =JSON.parse(JSON.stringify(data))
		a.result.goods_list = [];
		for(var i=0; i<  data.result.goods_list.length;i++){ 
			a.result.goods_list[i]={quantity:data.result.goods_list[i].quantity, id:data.result.goods_list[i].id, goods_name:data.result.goods_list[i].goods_name, sold_quantity:data.result.goods_list[i].sold_quantity, sku_list:[{skuId:data.result.goods_list[i].sku_list[0].skuId, skuQuantity:data.result.goods_list[i].sku_list[0].skuQuantity}]} 
		}
        chrome.runtime.sendMessage({type: 'postcookie',url:api_url_sj_stock,data:{'data':JSON.stringify(a)}}, function(response) {
            if(response == undefined)
			{
				log("商品列表数据发送失败");
			}else if(response.status){
                if(response.result.code == 1){
                    log(response.result.msg);
                }else{
                    log(response.result.msg);
                }
            }else{
                log("网络异常");
            }
        });

    }catch (e) {

    }
}

const details_load=()=>{
    chrome.runtime.sendMessage({type: 'get',url:api_url_details_get}, function(response) {
        if(response == undefined)
		{
			log("采集停止");
		}
		else if(response.status){
			if(response.result.code == 1){
				log(response.result.data);
				setTimeout(()=>{
					location.href = response.result.data;
				},1000)
			}else{
				log(response.result.msg);
				setTimeout(()=>{
					details_load()
				},1000)
			}
        }else{
            log("网络异常");
        }
    });
}



const script = document.createElement('script');
script.setAttribute('type', 'text/javascript');
script.setAttribute('src', chrome.extension.getURL('js/hook.js'));
document.documentElement.appendChild(script);

window.addEventListener("hook", function(event) {
    var url = event.detail.url;
    var data =event.detail.data;

    //治理重复回调问题
    if(data  == ''){return;}
    if(!window._cache){window._cache = '';}
    if(window._cache == data){return;}
    window._cache = data;
	if(window.caijistart == undefined){
		get_caijistart();	
	}
	if(window.caijistart){
		if(url.indexOf('/proxy/api/api/caterham/query/subfenlei_gyl_label') > -1){
			log("进入商品列表页subfenlei_gyl_label");
			load_good_list(data);
		}

		if(url.indexOf('/proxy/api/api/caterham/query/fenlei_gyl_group') > -1){
			log("进入商品列表页fenlei_gyl_group");
			load_good_list_index_cate(data);
		}

		
		if(url.indexOf('/proxy/api/search?') > -1){
			log("进入商品列表页search");
			load_good_list_search(data);
			if(window.autocaiji && document.getElementsByClassName("_2gTyX8PN")[0].innerText != window.caijicontent){
				if(window.interval){
					window.clearInterval(interval);
				}
				window.location.href ="http://mobile.pinduoduo.com/index.html";
			}
		}
		
		if(url.indexOf('/search_result.html?') > -1){
			log("进入商品列表页search_hook");
			setTimeout(()=>{
				var list = document.getElementsByTagName("script");
				for(var i=0;i< list.length;i++ ){ 
					if(list[i].innerText.indexOf("window.rawData=")>-1){
						var rawData = list[i].innerText.substr(0, list[i].innerText.indexOf("};"))
						load_good_list_search_result(rawData.replace("window.rawData=","")+"}",  "search_result" );
					}
				};
			},3000);
			
		}
		
		
		
	}
    if(url.indexOf('/home') > -1){
			log("进入商品列表页subfenlei_gyl_label");
			load_good_list(data);
		}
	if(window.kcxg){
		if(url.indexOf('vodka/v2/mms/query/display/mall/goodsList') > -1){
			log("进入商家商品列表");
			load_sj_good_list(data);
		}
	}

}, false);


var url=location.href;
if(url.indexOf('mms.pinduoduo.com/goods/goods_list') > -1){
	chrome.runtime.sendMessage({type: 'isstart'}, function(response) {
		const script = document.createElement('script');
			script.setAttribute('type', 'text/javascript');
			script.innerText="window.kcxg="+response.kcxg;
			document.documentElement.appendChild(script);  // 给hook.js发送消息
		window.kcxg = !!response.kcxg
		 log("商家后台商品列表,库存修改是否启动:"+window.kcxg);
	 });	
	
}
if(url.indexOf('mobile.pinduoduo.com') > -1){
    log("拼多多商城首页");
	get_caijistart();
	
}


if(url.indexOf('/search_result.html?') > -1 || url.indexOf('/catgoods.html?') > -1){ // 首页数据处理，不能hook到
	log("进入搜索商品列表页");
	(async function() {
		if(window.caijistart == undefined){
			get_caijistart();	
		}
		while(window.caijistart == undefined){
			await sleep(100);
		}
		if(window.caijistart){
			var list = document.getElementsByTagName("script");
			for(var i=0;i< list.length;i++ ){ 
				if(list[i].innerText.indexOf("window.rawData=")>-1){
					var rawData = list[i].innerText.substr(0, list[i].innerText.indexOf("};"))
					load_good_list_search_result(rawData.replace("window.rawData=","")+"}", url.indexOf('/catgoods.html?') > -1 ? "catgoods":"search_result" );
				}
			};
			if(window.interval){
				log("clearInterval");
				clearInterval(window.interval);
			}
			window.interval = setInterval(()=>{
				
				get_caijistart();
				if(window.autocaiji && (window.caijisize>0 || window.caijinewgoods>0)){
					log("执行"+i);
					window.scrollBy(i ,i +500);
					i  += 500;
				}
				else if(window.autocaiji) // 采集完成，开始评价采集
				{
					setTimeout(()=>{
						if(window.autocaiji && (window.caijisize>0 || window.caijinewgoods>0)){
							details_load();
						}
					}, 2000)
					
				}
				if(!window.caijistart)
				{
					clearInterval(window.interval);
				}
			},2000)
		}
	})();	
        
}
if(url.indexOf('/goods.html?goods_id=') > -1){
	(async function() {
		if(window.caijistart == undefined){
			get_caijistart();	
		}
		while(window.caijistart == undefined){
			await sleep(100);
		}
		if(window.caijistart){
			var goods_id = getQueryVariable('goods_id')
			log("商品详情页："+goods_id);
			var pd = '';
			var pl = '';
			try{
				pd = document.getElementsByClassName('_2TTv6US_')[0].innerText;
				pl = document.getElementsByClassName('ccIhLMdm')[0].innerText;
			}catch(e){
				
			}
			
			log("拼单人数："+pd)
			log("评论人数："+pl)

			chrome.runtime.sendMessage({type: 'postjson',url:api_url_details_set,data:{goods_id:goods_id,pd:pd,pl:pl}}, function(response) {
					if(response == undefined)
					{
						log("采集停止");
					}else if(response.status){
						if(response.result.code == 1){
							log(response.result.msg);
						}else{
							log(response.result.msg);
						}
						details_load()
					}else{
						log("网络异常");
					}
				});
		}
	})();
}

if(url.indexOf('mms.pinduoduo.com/home') > -1 || url.indexOf('mms.pinduoduo.com/supplier') > -1){ // 
	log("cookie");
	(async function() {
		chrome.runtime.sendMessage({type: 'cookie',url:host+"mille",data:{}}, function(response) {
					if(response && response.status){
						log(response.result.msg);
					}else{
						log("网络异常");
					}
				});
	})();
}
var i = 0;

//已放弃
const goods_update=(i)=>{
	(async function() {
		await sleep(2000);
	})();
	
}


if(url.indexOf('mobile.pinduoduo.com/index.html') > -1){
    log("首页");
	chrome.runtime.sendMessage({type: 'isstart'}, function(response) {
		if(response == undefined || !response.caijistart || !response.autocaiji)
		{
			log("采集停止");
			return;
		}
		setTimeout(()=>{
			var e = document.createEvent("MouseEvents");
			e.initEvent("click", true, true);
			document.getElementsByClassName("_2fnObgNt")[0].dispatchEvent(e);
		},2000)
	 });	
	
}
if(url.indexOf('mobile.pinduoduo.com/relative_goods.html') > -1){
    log("搜索");
	chrome.runtime.sendMessage({type: 'isstart'}, function(response) {
		if(response == undefined || !response.caijistart|| !response.autocaiji)
		{
			log("采集停止");
			return;
		}
		setTimeout(()=>{
			var submit = document.getElementById("submit");
			if(submit && submit.firstElementChild)
			{
				submit.firstElementChild.value=window.caijicontent;
				var e = document.createEvent("MouseEvents");
				e.initEvent("click", true, true);
				document.getElementsByClassName("RuSDrtii")[0].dispatchEvent(e);
			}
			
        
		},2000)
	 });	
	
}


