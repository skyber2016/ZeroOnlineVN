function tfid(id) {
	return document.getElementById(id);
}

//tab�л�
function tfchg(BTN, TXT) {
	var btns = tfid(BTN).getElementsByTagName("li");
	var txts = tfid(TXT).getElementsByTagName("ol");
	for (i = 0; i < txts.length; i++) {
		btns[i].index = i;
		btns[i].onclick = function () {
			chage(this);
		}
	}

	function chage(o) {
		for (j = 0; j < txts.length; j++) {
			btns[j].className = j == o.index ? "on" : "";
			txts[j].style.display = j == o.index ? "block" : "none";
		}
	}
}

function dhmenuchg(obj, n) {//�����˵�
	var dhul = obj.getElementsByTagName("ul")[0];
	dhul.style.display = n == 1 ? "block" : "none";
	obj.style.position = n == 1 ? "relative" : "";
}

//�������ſ�ȹ̶�
function setTdWidth(obj) {
	var items = document.getElementById(obj).getElementsByTagName("td");
	for (var i = 0; i < items.length; i += 3) {
		items[i + 1].style.width = "73%";
		items[i + 2].style.width = "16%";
		items[i + 2].style.color = "#686161"
	}
}

//��ҳ�ֲ�
function playFlash(flashid, iUrl, iWmode, iWidth, iHeight, flashboxid) {
	var fpic = document.getElementById(flashid).getElementsByTagName("img");
	var flink = document.getElementById(flashid).getElementsByTagName("a");
	var imag = new Array();
	var link = new Array();
	for (var i = 0; i < fpic.length; i++) {
		imag[i] = fpic[i].src;
		link[i] = flink[i].href;
	}

	//�ɱ༭���ݽ���

	var flashvars = "";
	for (var i = 0; i < imag.length; i++) {
		if (i == imag.length - 1) {
			flashvars = flashvars + (imag[i] + "#" + link[i]);
		} else {
			flashvars = flashvars + (imag[i] + "#" + link[i] + "|");
		}
	}
	//alert(imag.length);
	flash = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="' + iWidth + '" height="' + iHeight + '" >';
	flash = flash + '<param name="movie" value="' + iUrl + '" />';
	flash = flash + '<param name="quality" value="high" />';
	flash = flash + '<param name="menu" value="false" />';
	if (iWmode == 1) {
		flash = flash + '<param name="wmode" value="transparent" />';
	}
	flash = flash + '<param name="FlashVars" value="mylinkpic=' + flashvars + '" />';
	flash = flash + '<embed src="' + iUrl + '" width="' + iWidth + '" height="' + iHeight + '" menu="false" quality="high" ';
	if (iWmode == 1) {
		flash = flash + 'wmode="transparent" ';
	}
	flash = flash + ' pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" wmode="transparent" flashvars="mylinkpic=' + flashvars + '"></embed>';
	flash = flash + '</object>';

	document.getElementById(flashboxid).innerHTML = flash;

}


//����ʽ���
function Pop() {
	//������
	var isLeft = false; //���춨λ��trueΪ��ߣ�falseΪ�ұߣ�
	var finalHeight = 360; //�������ո߶�
	var step = 9; //�����沽��������Խ��Խ��
	var lapse = 1; //��ʱ������ԽСԽ��
	var isSlowClose = false; //�Ƿ����رգ�trueΪ�������͹رգ�falseΪ�����رգ�
	var GGlapse = 2000; //2���Ӻ�ʼ������
	var isIE6 = (navigator.appVersion.indexOf("MSIE 6") > -1) ? true : false;
	var pos = null;
	if (isIE6)
		document.writeln("<style type=\"text/css\">#PopContext{position:absolute;bottom: auto;clear: both;top:expression(eval(document.compatMode && document.compatMode=='CSS1Compat') ? documentElement.scrollTop+(documentElement.clientHeight-this.clientHeight) - 1 : document.body.scrollTop+(document.body.clientHeight-this.clientHeight) - 1);}</style>");
	var time, he, obj;
	var div = document.getElementById("PopContext");
	div.style.position = (isIE6) ? "absolute" : "fixed";
	div.style.zIndex = "100";
	div.style.bottom = "0px";
	div.style.overflow = "hidden";
	(isLeft) ? div.style.left = "0px" : div.style.right = "0px";
	div.style.height = "0px";

	var shut = document.getElementById("shut");
	if (shut != null)
		shut.onclick = setClose;

	function setClose() {
		clearTimeout(time);
		if (isSlowClose) {
			reduce();
		} else {
			div.style.display = "none";
		}
	}

	function add() {
		he = getHeight(div.style.height);
		if (he < finalHeight) {
			div.style.display = "block";
			he += step;
			div.style.height = he.toString() + "px";
			time = setTimeout(add, lapse);
		} else {
			div.style.height = finalHeight.toString() + "px";
			clearTimeout(time);
		}
		if (isIE6) {
			div.style.bottom = "0px";
		}
	}

	function reduce() {
		he = getHeight(div.style.height);
		if (he > 0) {
			he -= step;
			div.style.height = (he < 0) ? "0px" : he.toString() + "px";
			time = setTimeout(reduce, lapse);
		} else {
			div.style.height = "0px";
			div.style.display = "none";
			clearTimeout(time);
		}
		if (isIE6) {
			div.style.bottom = "0px";
		}
	}

	function getHeight(divHeight) {
		return parseInt(divHeight.replace("px", ""));
	}

	setTimeout(add, GGlapse);
}

function clo() {
	document.getElementById("tc").style.display = "none";
}
