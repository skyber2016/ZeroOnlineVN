function showFlash(a, b, c, d) {
	flash = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0" width="' + b + '" height="' + c + '" >';
	flash = flash + '<param name="movie" value="' + a + '" />';
	flash = flash + '<param name="quality" value="high" />';
	flash = flash + '<param name="menu" value="false" />';
	flash = flash + '<param name="allowScriptAccess" value="always" />';
	flash = flash + '<param name="allowFullscreen" value="true" />';
	if (d == 1) {
		flash = flash + '<param name="wmode" value="transparent" />'
	}
	flash = flash + '<embed src="' + a + '" width="' + b + '" height="' + c + '" allowScriptAccess="always" allowFullscreen="true" menu="false" quality="high" ';
	if (d == 1) {
		flash = flash + 'wmode="transparent" '
	}
	flash = flash + ' pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash"></embed>';
	flash = flash + "</object>";
	document.writeln(flash)
}

function MM_swapImgRestore() {
	var d, b, c = document.MM_sr;
	for (d = 0; c && d < c.length && (b = c[d]) && b.oSrc; d++) {
		b.src = b.oSrc
	}
}

function MM_preloadImages() {
	var f = document;
	if (f.images) {
		if (!f.MM_p) {
			f.MM_p = new Array()
		}
		var e, c = f.MM_p.length,
			b = MM_preloadImages.arguments;
		for (e = 0; e < b.length; e++) {
			if (b[e].indexOf("#") != 0) {
				f.MM_p[c] = new Image;
				f.MM_p[c++].src = b[e]
			}
		}
	}
}

function MM_findObj(f, e) {
	var c, b, a;
	if (!e) {
		e = document
	}
	if ((c = f.indexOf("?")) > 0 && parent.frames.length) {
		e = parent.frames[f.substring(c + 1)].document;
		f = f.substring(0, c)
	}
	if (!(a = e[f]) && e.all) {
		a = e.all[f]
	}
	for (b = 0; !a && b < e.forms.length; b++) {
		a = e.forms[b][f]
	}
	for (b = 0; !a && e.layers && b < e.layers.length; b++) {
		a = MM_findObj(f, e.layers[b].document)
	}
	if (!a && e.getElementById) {
		a = e.getElementById(f)
	}
	return a
}

function MM_swapImage() {
	var e, d = 0,
		b, c = MM_swapImage.arguments;
	document.MM_sr = new Array;
	for (e = 0; e < (c.length - 2); e += 3) {
		if ((b = MM_findObj(c[e])) != null) {
			document.MM_sr[d++] = b;
			if (!b.oSrc) {
				b.oSrc = b.src
			}
			b.src = c[e + 2]
		}
	}
};


function logoimg(jz) {
	if (jz == undefined || jz == "pc") {
		document.write("<a href='https://jz.99.com/index/' target='_blank' title='��ս������'><img onload='transPNGPic(this);' src='https://img5.99.com/jz/index/logo/logo.png'/></a>")
	} else if (jz == "pad") {
		document.write("<a href='http://wxjz.99.com/' target='_blank' title='��սPad����'><img onload='transPNGPic(this);' src='https://img5.99.com/wxjz/images/logo/logo.png'/></a>");
	} else if (jz == "jsz") {
		document.write("<a href='https://jz.99.com/index/' target='_blank' title='��ս������'><img onload='transPNGPic(this);' src='https://img4.99.com/jz/index/logo/jzSaviorLogo.png'/></a>");
	}
}

var setLogo = function () {
	var logo = '<a href="https://jz.99.com/index/" title="��ս���Ĺٷ���վ"><img src="https://img5.99.com/jz/index/logo/918/logo.png" alt="��ս���Ĺٷ���վ" /></a>';
	var eleLogo = document.getElementById('logo');
	if (eleLogo) {
		eleLogo.innerHTML = logo;
	} else {
		document.write(logo);
	}
};

function tongji() {
	document.write("<script src='https:\/\/w.cnzz.com\/c.php?id=30072404' language='JavaScript'><\/script>")
}

function pings() {
	document.write("<script src='https:\/\/w.cnzz.com\/c.php?id=30072404' language='JavaScript'><\/script>")
}
