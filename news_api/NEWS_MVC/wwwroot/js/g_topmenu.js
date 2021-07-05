//topMenu--start
function topGetE(o){
    return document.getElementById(o);
}
function show_public_top(_this){
    _this.className="public_top_menu_s public_top_menu_on";
    _this.getElementsByTagName("a")[0].className="public_top_menu_t public_top_menu_on";
    _this.getElementsByTagName("div")[0].style.display="block";
}

function hide_public_top(_this){
    _this.className="public_top_menu_s";
    _this.getElementsByTagName("a")[0].className="public_top_menu_t";
    _this.getElementsByTagName("div")[0].style.display="none";
}
function show_log_menu(){
    _this = topGetE('user_area');
    _this.className = "public_top_link_loged public_top_link_loged_on";
    topGetE('log_menu').style.display="block";
    topGetE('log_menu').style.visibility="visible";
}

function hide_log_menu(){
    _this = topGetE('user_area');
    _this.className=_this.className.replace(" public_top_link_loged_on","");
    topGetE('log_menu').style.display="none";
    topGetE('log_menu').style.visibility="hidden";
}
function show_task_menu(_this){
    /*_this.className = "public_top_link_task public_top_link_task_on";
    _this.getElementsByTagName("div")[0].style.display="block";*/
}

function hide_task_menu(_this){
    /*_this.className = "public_top_link_task";
    _this.getElementsByTagName("div")[0].style.display="none";*/
}
function showlogbox(){
    topGetE('logbox').style.display = topGetE('logbox').style.display=="block"?"none":"block";
    topGetE('logbg_ing').style.display = 'none';
    topGetE('logbg_ing').style.width = '0';
    topGetE('logbg_ing').style.width = '304px'; //fix for ie
}

var sqhdTitle = "";//社区活动
var sqhdHref = "";//社区活动
var btnnoshow=1;

function topMenu(type,target){
    //暗色官网 type = "black"
    //target默认"_top"
    //init
    var logoTitle="www.99.com";
    var logoHref="https://www.99.com/";
    var imgSiteUrl="https://img5.99.com/news/images/topmenu/0620/";
    var siteImg="split_v2_160129.gif";//导航split图
    var menubg = "menubg3.gif?V1";//菜单背景
    target = (target == "_blank" || target == "_self" || target == "_top" || target == "_parent") ? target : "_top";

    var skinImg="skin_210517.jpg";//皮肤，节日背景图【为空自动取消】
    var skinPanel = "big_210517.jpg"; //皮肤节日大图
    var skinTitle="探寻山海大荒 新资料片预约有礼";//皮肤，节日背景图，标题
    var skinHref="https://act.my.99.com/shjyy/";//皮肤，节日背景图，链接

    if(/mykd/i.test(window.location.href)){
        skinImg="skin_210618.jpg";//皮肤，节日背景图【为空自动取消】
        skinPanel = ""; //皮肤节日大图
        skinTitle="魔域口袋";//皮肤，节日背景图，标题
        skinHref="https://mykd.99.com/";//皮肤，节日背景图，链接
    }

    if(/kx/i.test(window.location.href)){
        skinImg="skin_190307_kx.jpg";//皮肤，节日背景图【为空自动取消】
        skinPanel = ""; //皮肤节日大图
        skinTitle="开心";//皮肤，节日背景图，标题
        skinHref="https://kx.99.com/index";//皮肤，节日背景图，链接
    }

    if(/zf/i.test(window.location.href)){
        skinImg="skin_180416_zf.jpg";//皮肤，节日背景图【为空自动取消】
        skinPanel = ""; //皮肤节日大图
        skinTitle="聚宝阁";//皮肤，节日背景图，标题
        skinHref="https://cbg.99.com/";//皮肤，节日背景图，链接
    }

    var hbqStyle="";
    if(/hbq/i.test(window.location.href)){
        skinImg="skin_20180130_hbq.jpg";//皮肤，节日背景图【为空自动取消】
        skinPanel = ""; //皮肤节日大图
        skinTitle="虎豹骑";//皮肤，节日背景图，标题
        skinHref="https://hbq.99.com/";//皮肤，节日背景图，链接
        hbqStyle="display:none !important;";
    }
    var czStyle="";
    if(/yhkd/i.test(window.location.href)){
        skinImg="skin_210611_yhkd.jpg";//皮肤，节日背景图【为空自动取消】
        skinPanel = ""; //皮肤节日大图
        skinTitle="HER系列皮肤";//皮肤，节日背景图，标题
        skinHref="https://yhkd.99.com/act/2021/her-skin/";//皮肤，节日背景图，链接
        czStyle="display:none;";
    }
    if(/yhzl/i.test(window.location.href)){
        skinImg="skin_210422_yhzl.jpg";//皮肤，节日背景图【为空自动取消】
        skinPanel = ""; //皮肤节日大图
        skinTitle="英魂之刃战略版";//皮肤，节日背景图，标题
        skinHref="https://yhzl.99.com/news/04222021/030154968.shtml";//皮肤，节日背景图，链接
        czStyle="display:none;";
    }
    if(/kdzf/i.test(window.location.href)){
        czStyle="display:none;";
    }

    var gameIco = "gameico_0603.gif"; //游戏图标
    var panelHeight = "180px"; //列表高度
    var affiche="";//重要公告【为空自动取消】

    //暗色官网
    if(type == "black"){
        siteImg="split_b.gif";
        menubg = "menubg2.gif";
        skinImg="skin_0829.jpg";//皮肤，节日背景图【为空自动取消】
        gameIco = "gameico_b_0722.gif";
    }
    //增值专用
    var btnStyle="";
    if(type=="shop"|type=="aq"){
        btnStyle="display:none;";
        btnnoshow=0;
    }
    //
    var skinHtml="";
    if(skinImg==null || skinImg==""){
        skinImg="none";
    }else{
        skinImg="url("+imgSiteUrl+skinImg+");";
        skinHtml="<a target='_blank' title='"+skinTitle+"' href='"+skinHref+"' rel='noopener'>"+skinTitle+"</a>";
    }
    if(skinPanel!=null && skinPanel!=""){
        skinHtml += "<div class='public_top_skinpanel' style='background:url(" + imgSiteUrl + skinPanel + ") no-repeat right -46px; position:absolute; right:0; top:40px; display:none; border:1px solid #CECECE; border-top:0;'><a style='width:560px; height:172px; display:block;' target='_blank' title='"+skinTitle+"' href='"+skinHref+"' rel='noopener'>"+skinTitle+"</a></div>";
    }
    var afficheStyle="",sqhdStyle="";
    if(affiche==null || affiche==""){
        afficheStyle="display:none;";
    }
    if(sqhdTitle==null || sqhdTitle==""){
        sqhdStyle="display:none;";
    }
    document.write(
        "<style type='text/css'>"+
        /* float */
        ".clearfix:after{ content:''; height:0; visibility:hidden; display:block; clear:both;}"+
        ".clearfix{ zoom:1;}"+
        ".clear { clear:both }"+

        /*full,box*/
        ".public_top_full_bg{height:45px;position:relative;z-index:10000;text-align:center;background:url("+imgSiteUrl+siteImg+") repeat-x left -135px;}"+
        ".public_top_box{height:45px;margin:0 auto;width:1176px;padding:0 11px;font-size:12px;background:url("+imgSiteUrl+siteImg+") repeat-x left -11145px;color:#333;}"+
        /*reset*/
        "#public_top .public_top_link_loged img{vertical-align: top;}"+
        ".public_top_box ul,.public_top_box li,.public_top_box dl,.public_top_box dt,.public_top_box dd{margin:0;padding:0;}"+
        ".public_top_box li{list-style:none;}"+
        ".public_top_box a,.public_top_box a:hover{text-decoration:none;}"+
        /*public*/
        ".public_top_logo,.public_top_menu,.public_top_skin,.public_top_link{float:left;height:45px;}"+
        /*logo*/
        "div.public_top_logo{width:150px;background:url("+imgSiteUrl+siteImg+") no-repeat 0 0; display:block;}"+
        "div.public_top_logo a{height:45px;display:block;text-indent:-999em;}"+
        /*menu*/
        "ul.public_top_menu{width:200px;}"+
        /*menu single*/
        "li.public_top_menu_s{height:45px;float:left;position:relative; background:url("+imgSiteUrl+siteImg+") no-repeat 5px -400px; padding-left:10px; width:101px;}"+
        "li.public_top_menu_on{background:url("+imgSiteUrl+siteImg+") no-repeat 5px -445px; z-index:9999;}"+
        "li.public_top_menu_s a.public_top_menu_t{height:45px;line-height:48px;display:block;color:#727272;background:url("+imgSiteUrl+siteImg+") no-repeat right -400px;padding:0 19px 0 5px; overflow:hidden;}"+
        "li.public_top_menu_s a.public_top_menu_t:hover{background:url("+imgSiteUrl+siteImg+") no-repeat right -445px;}"+
        "li.public_top_menu_s a.public_top_menu_on{background:url("+imgSiteUrl+siteImg+") no-repeat right -445px; color:#019BD0;}"+

        /*skin*/
        "div.public_top_skin{width:490px; height:44px; top:0px; background:url("+imgSiteUrl+skinImg+") no-repeat center center; position:relative;}"+
        "div.public_top_skin a{height:45px;display:block;text-indent:-999em;}"+
        "div.public_top_skin a.public_top_menu_on{height:41px;display:block;text-indent:-999em; background:url("+imgSiteUrl+skinImg+") repeat-x left -139px}"+
        "ul.public_top_link{float:right;}"+
        "#user_login{ position:relative} "+
        "ul.public_top_link li{height:45px;line-height:45px;float:left;width:auto; padding:0 7px; _padding:0 7px; text-align:center;}"+
        "ul.public_top_link li.pipe{background:url(" + imgSiteUrl + siteImg + ") no-repeat -149px -180px;}"+
        "ul.public_top_link li a{color:#727272;}"+
        "ul.public_top_link li a:hover{color:#019bd0;text-decoration:underline;}"+
        "ul.public_top_link li a.public_top_link_log{ background:url(" + imgSiteUrl + siteImg + ") no-repeat 0 -180px; display:block; color:#FFF; width:78px;}"+
        "ul.public_top_link li a.public_top_link_log:hover{ text-decoration:none; color:#FFF;}"+
        "ul.public_top_link .public_top_link_loged{background:url(" + imgSiteUrl + siteImg + ") no-repeat right -225px; width:96px;}"+
        "ul.public_top_link .public_top_link_loged_on{ background:url(" + imgSiteUrl + siteImg + ") no-repeat right -315px; width:96px;}"+
        "ul.public_top_link .public_top_link_message{  background:url(" + imgSiteUrl + siteImg + ") no-repeat 26px -315px; _background:url(" + imgSiteUrl + siteImg + ") no-repeat 16px -315px;  width:47px;}"+
        "ul.public_top_link .public_top_link_message_on{ background-position:right -315px;}"+
        "ul.public_top_link .userhead{float:left; padding:12px 7px 0 3px;}"+
        "ul.public_top_link .usercount{ float:left; height:45px; overflow:hidden;}"+
        "ul.public_top_link .public_top_link_message a{display:block;}"+
        "ul.public_top_link .public_top_link_loged_on .rarr{ background:url(" + imgSiteUrl + siteImg + ") no-repeat -72px -225px; width:32px;}"+
        "ul.public_top_link .public_top_msg_num{ background:url(" + imgSiteUrl + siteImg + ") no-repeat 0 -360px; display:block; width:15px; height:17px; line-height:11px; color:#FFF; font-size:10px; position:absolute; right:15px; top:5px; font-family:Arial;}"+
        ".public_top_link_loged,.public_top_link_task{position:relative;}"+
        "ul.public_top_link .public_top_link_task{background:url(" + imgSiteUrl + siteImg + ") no-repeat right -225px; width:32px; padding:0 20px 0 10px; position:relative;}"+
        "ul.public_top_link .public_top_link_task_on{background-position: right -270px;}"+
        "ul.public_top_link .public_top_link_task_on a{color:#019bd0;}"+
        "ul.public_top_link .public_top_link_task .task_new{ position:absolute; right:0; top:5px;}"+
        ".menu_task{ background:#FFF; width:270px; display:block; border:1px solid #ccc; border-top:none; padding:0 10px 5px; position:absolute; top:45px; right:0; text-align:left;}"+
        ".menu_task a{ color:#019bd0;}"+
        ".menu_task .task_hd{ height:30px; line-height:30px; border-bottom:1px solid #ccc; padding:0 5px;}"+
        ".menu_task .task_hd_more{float:right;}"+
        ".menu_task .task_li{ line-height:30px; }"+
        ".menu_task .task_li div{float:left; width:260px;padding:0 5px; border-bottom:1px dashed #ccc;}"+
        ".menu_task .task_li div.last{ border:none;}"+
        ".menu_task .task_li a{float:left; width:130px; overflow:hidden;}"+
        ".menu_task .task_li a:hover{text-decoration:none; color:#f00;}"+
        ".menu_task .task_li .task_li_jf, .menu_task .task_li .task_li_exp{ float:right; width:55px; text-align:left;}"+
        ".menu_task .task_li .task_li_jf em, .menu_task .task_li .task_li_exp em{float:left; width:20px; height:20px; background:url(" + imgSiteUrl + siteImg + ") no-repeat -15px -360px; margin:5px 0; *display:inline;}"+
        ".menu_task .task_li .task_li_exp em{width:30px; background-position: -35px -360px;}"+
        ".log_menu{position:absolute; right:0; top:45px; border:1px solid #CCC; border-top:0; width:270px; text-align:left; padding:10px 10px 3px; line-height:24px; background-color:#FFF;}"+
        ".log_menu .menu_opt{float:left; width:185px;}"+
        ".log_menu .menu_opt0{padding:0 0 0 10px;}"+
        ".log_menu .menu_opt0 a{color:#09A5D4;}"+
        ".log_menu .menu_opt1{float:left; width:100px;}"+
        ".log_menu .menu_opt1 a{ display:block; line-height:30px; padding-left:10px;}"+
        ".log_menu .menu_opt2{float:right; width:85px;}"+
        ".log_menu .menu_opt2 .menu_jf{display:block; height:30px;}"+
        ".log_menu .menu_opt2 .menu_jf em{display:inline-block; *display:inline; zoom:1; width:20px; height:20px; background:url(" + imgSiteUrl + siteImg + ") no-repeat -15px -360px; vertical-align:middle;}"+
        ".log_menu .menu_opt2 .menu_btnqd{display:block; width:76px; height:27px; line-height:27px; text-align:center; color:#fff; background:url(" + imgSiteUrl + siteImg + ") no-repeat 0 -491px; }"+
        ".log_menu .menu_opt2 .menu_btnqd:hover{ background-position:0 -518px; text-decoration:none; color:#fff}"+
        ".log_menu .menu_opt2 .menu_btnqd_disable{display:none; width:76px; height:27px; line-height:27px; text-align:center; color:#fff; background:#d0c09a;  text-decoration:none;}"+
        ".log_menu .menu_opt2 .menu_btnqd_disable:hover{color:#fff; text-decoration:none}"+
        ".log_menu .menu_opt2 .menu_qdinfo{ padding:2px 0 5px 0; color:#ED5325; text-align:center; width:85px;}"+
        ".log_menu .menu_optb{ clear:both; background:#F2F3F2; padding:3px 5px; display:none}"+
        ".log_menu .menu_optb a{float:left; width:118px; height:24px; overflow:hidden; padding:0 0 0 10px; display:none}"+
        ".log_menu .menu_optb a span{color:#ED5325; font-weight:100;}"+
        ".log_menu .menu_btm{border-top:1px solid #ccc; height:30px; padding:10px 0 0 0; position:relative}"+
        ".log_menu .menu_btm .sqhd{color:#FF3300}"+
        ".log_menu .menu_btm .top_logout{background:url(" + imgSiteUrl + siteImg + ") no-repeat -76px -491px; position:absolute; right:0; top:10px; width:53px; height:23px; line-height:23px; text-align:center}"+
        ".log_menu .menu_btm .top_logout:hover{background-position:-76px -514px; text-decoration:none;}"+
        ".log_menu .menu_head{float:left; width:85px;}"+
        ".log_menu_qdmsg{position:absolute; top:140px; left:-10px; border:1px solid #E8E7D5; padding:5px; background-color:#FFFFCD; width:120px; line-height:20px; color:#B39853; z-index:999}"+
        ".btnqd{display:block; width:62px; height:42px; line-height:42px; text-align:center; color:#fff; background:url(" + imgSiteUrl + "btn_sprite.gif) no-repeat -69px -81px;  position:absolute; top:35px; right:12px;}"+
        " .btnqd:hover{ color:#FFF;}"+
        " .btnqd_disable{display:none; width:62px; height:42px; line-height:42px; text-align:center; color:#fff; background:#d0c09a; text-decoration:none; position:absolute; top:35px; right:12px; cursor:default;}"+
        " .btnqd_disable:hover{color:#fff; text-decoration:none}"+
        " .qdinfo{ color:#ED5325; text-align:center; width:85px; position:absolute; top:78px; right:0;}"+
        ".qdpop{background:url(" + imgSiteUrl + "qdpop1018.gif) no-repeat #83d7dd; width:526px; /*min-height:415px;*/ font-size:12px; z-index:10001;  }"+
        "#qdpop_ing{position:absolute; left:5px; top:35px; background:#FFF; color:#666; width:510px; height:270px; line-height:270px; _height: 140px; _padding-top:130px;text-align:center; display:none}"+
        ".qdpop_hd{line-height:46px; height:46px; padding:0 10px; font-size:18px; color:#fff; text-align:center; font-weight:bold;}"+
        ".qdpop_hd .qdpop_shut{float:right; margin:8px 0 0 0; *display:inline; width:18px; height:18px; line-height:18px; color:#959595; border:1px solid #c1c1c1; background-color:#FFF; text-decoration:none; text-align:center; font-family:Tahoma; font-size:14px;}"+
        ".qdpop_hd .qdpop_shut:hover{ background:#ccc; color:#FFF;}"+
        ".qdpop_bd{padding:24px 25px 5px 25px; text-align:left;}"+
        ".qdpop_bd p{margin:0;padding:0;}"+
        ".qdpop_bd ul,.qdpop_bd li{margin:0;padding:0; list-style:none;}"+
        ".qdpop_bd a{color: #2584b0; text-decoration:none;}"+
        ".qdpop_bd a:hover {color: #ff6320; text-decoration:underline;}"+
        " #qd_tip{height:48px; line-height:25px; color:#434343; font-size:14px;}"+
        " .qd_face{height:52px; background:#f5f7f6; border:1px solid #efefef; padding:1px;}"+
        " .qd_face li{float:left; padding:5px 10px; list-style:none; text-align:center; width:27px; cursor:pointer;}"+
        " .qd_face li.on{background-color:#eee;}"+
        " .qd_face li span{display:block; color:#666; padding:5px 0 0 0;}"+
        " .qb_mood{line-height:26px; position:relative; padding:8px 9px 0px 9px; color:#949494; height:105px;}"+
        " .qd_myword{width:445px; height:102px;background:#FFF url(" + imgSiteUrl + "bg_input.gif) no-repeat; line-height:24px; padding:0 5px; border:1px solid #dbdbdb;border-radius:2px; -webkit-transition: box-shadow .1s linear; -moz-transition: box-shadow .1s linear; outline:none; -webkit-outline-radius:0;  vertical-align:middle; _margin-bottom:5px; color:#949494; font-size:12px; }"+
        " .qb_mood span{ position:absolute; bottom:2px; right:30px;}"+
        " .qd_myword:focus{border:1px solid #9BBBDA; box-shadow:0 0 5px rgba(110, 189, 222, 0.5); -webkit-box-shadow:0 0 5px rgba(110, 189, 222, 0.5);}"+
        " .qd_btn{ min-height:40px; padding:5px 10px 0;_height:40px;}"+
        " .qd_btn span{ display:inline-block; float:left;}"+
        " .qd_btn .ltc1{ line-height:31px; margin-right:3px; overflow:hidden; width:55px}"+
        " .qd_btn .ltc2{ width:200px; position:relative;}"+
        " .qd_btn .ltc2 input{ width:60px; background: url(" + imgSiteUrl + "bg_input.gif) no-repeat 0 0 #FFFFFF; border: 1px solid #DBDBDB; border-radius: 2px; height: 22px; line-height: 22px; outline: none; padding: 3px; transition: box-shadow 0.1s linear 0s; vertical-align: top;}"+
        " .qd_btn .ltc2 img{ position:absolute; top:5px; left:70px; cursor:pointer; }"+
        " .qd_btn a{font-size:13px;}"+
        " .qd_btn a.huati {background:url(" + imgSiteUrl + "pdpop_ico.gif) no-repeat -214px 2px; padding-left:23px; color:#0097b3; text-align:left; height:22px; line-height:22px; font-size:13px; float:left;}"+
        " .qd_btn a.fu_qd{background:url(" + imgSiteUrl + "pdpop_ico.gif) no-repeat 0px -35px; color:#0097b3; text-align:left; height:36px; line-height:36px;text-align:center;  width:102px; color:#FFF; display:block; float:right; font-weight:bold;}"+
        " .qd_btn a.fu_qd:hover{ background:url(" + imgSiteUrl + "pdpop_ico.gif) no-repeat -103px -35px; text-decoration:none;}"+
        " #qb_mood_tip{ display:none; padding:6px 4px 4px 28px; font-size:12px; line-height:18px; background: url(" + imgSiteUrl + "error.gif) no-repeat; color:#F66; }"+
        " #qd_activity_news{margin-top:10px;}"+
        " #qd_activity_news a{ padding:0; display:inline;}"+
        "#qdwin{ padding:0 20px; _padding:5px 20px; font-size:12px; _width:420px; text-align:center; height:35px; line-height:35px; background:#FFF; color:#ED5325}"+
        "#qdwin a{ color:#019BD0; text-decoration:none}"+
        "#qdwin a:hover{ text-decoration:underline}"+
        ".public_menu_game_hd{ background:url(" + imgSiteUrl + menubg + ") no-repeat 0 0; float:left; padding-top:8px;  position:absolute; top:40px; left:6px; text-align:left; width:794px;background-color:#F2F2F2; }"+
        ".public_menu_game_dt{ height:35px; overflow:hidden;}"+
        ".public_menu_game_fd{ background:url(" + imgSiteUrl + menubg + ") no-repeat -1588px bottom; padding-bottom:8px; float:left; width:794px;}"+
        ".public_menu_game{ background:url(" + imgSiteUrl + menubg + ") repeat-y -794px 0;  padding:0 7px; float:left; width:780px;}"+
        ".public_menu_game_hd dl{ float:left; margin:0; display:block !important; overflow:hidden;}"+
        ".public_menu_game_hd dt{height:36px; line-height:36px; text-indent:10px; font-weight:700; font-size:13px; color:#0aa9d6;}"+
        ".public_menu_game_hd dt a{height:36px; line-height:36px; font-weight:700; font-size:13px; color:#0aa9d6;}"+
        ".public_menu_game_hd dt a:hover{text-decoration:underline;}"+
        ".public_menu_game_hd a{height:30px; line-height:30px; overflow:hidden; float:left; padding-left:5px; font-size:12px; color:#333; text-decoration:none; width:100%}"+
        ".public_menu_game_hd a.hover,.public_menu_game a:hover{color:#0AA9D6;}"+
        ".public_menu_game_hd a span{ font-size:11px}"+
        ".public_menu_game_hd dd a:hover{background-color:#f6f6f6;}"+
        ".public_menu_game_hd dd{border-right:1px solid #ccc; padding:10px !important; height:" + panelHeight + "; overflow:hidden;}"+
        ".public_menu_game_hd dd li{list-style:none; float:left; width:100px;}"+
        " .public_menu_game_col1{ width:300px;}"+
        " .public_menu_game_col2{ width:240px;}"+
        " .public_menu_game_col2 a{width:100px;}"+
        " .public_menu_game_col3{ width:300px;}"+
        " .public_menu_game_col4{ width:180px;}"+
        " .public_menu_game_col4 a{background:url("+imgSiteUrl+siteImg+") no-repeat -145px -360px; width:70px;}"+
        " .public_menu_game_col4 dd{border-right:0;}"+
        " .ico_game{ background-image:url("+imgSiteUrl+gameIco+"); background-repeat:no-repeat; float:left; width:20px; height:20px; margin:5px 5px 0 0; *display:inline;}"+
        " .ico_my{background-position:-20px 0;}"+
        " .ico_dk{background-position:-40px 0;}"+
        " .ico_jz{background-position:-60px 0;}"+
        " .ico_kx{background-position:-80px 0;}"+
        " .ico_yx{background-position:-100px 0;}"+
        " .ico_yyl{background-position:-120px 0;}"+
        " .ico_yhkd{background-position:-140px 0;}"+
        " .ico_hl{background-position:-160px 0;}"+
        " .ico_ty{background-position:-200px 0;}"+
        " .ico_gp{background-position:-220px 0;}"+
        " .ico_tmz{background-position:-280px 0;}"+
        " .ico_zf{background-position:-300px 0;}"+
        " .ico_wxy{background-position:-320px 0;}"+
        " .ico_wjzd{background-position:-340px 0;}"+
        " .ico_aoe{background-position:-360px 0;}"+
        " .ico_zjh{background-position:-400px 0px;}"+
        " .ico_kd{background-position:-420px 0px;}"+
        " .ico_hbq{background-position:-440px 0px;}"+
        " .ico_wh{background-position:-460px 0px;}"+
        " .ico_kdzf{background-position:-480px 0px;}"+
        " .ico_zsmy{background-position:-500px 0px;}"+
        " .ico_sssg{background-position:-140px -20px;}"+
        " .ico_qjp{background-position:-240px -20px;}"+
        " .ico_dsj{background-position:-300px -20px;}"+
        " .ico_cos{background-position:-360px -20px;}"+
        " .ico_xmj{background-position:-380px -20px;}"+
        " .ico_fkbl{background-position:-400px -20px;}"+
        " .ico_aoe1{background-position:-420px -20px;}"+
        " .ico_zcq{background-position:-480px -20px;}"+
        " .ico_myhz{background-position:-500px -20px;}"+
        " .ico_nc{background-position:-20px -40px;}"+
        " .ico_mc{background-position:-40px -40px;}"+
        " .ico_mykd{background-position:-100px -40px;}"+
        " .ico_zg{background-position:-120px -40px;}"+
        " .ico_tzsg{background-position:-140px -40px;}"+
        " .ico_fs{background-position:-180px -40px;}"+
        " .ico_yj{background-position:-220px -40px;}"+
        " .ico_sgsd{background-position:-360px -40px;}"+
        " .ico_sh{background-position:-420px -40px;}"+
        " .ico_yhzl{background-position:0px -20px;}"+
        "#tinybox {position:absolute; display:none; background:#FFF url(" + imgSiteUrl + "loading_bar.gif) no-repeat 50% 50%;  z-index:2000}"+
        "#tinycontent {background:#fff}"+
        ".pop_qd li{ float:left; margin-right:5px;}"+
        ".pop_qd li a{ display:block; width:106px; height:29px; line-height:29px; color:#FFF; background:url(" + imgSiteUrl + "pdpop_ico.gif) no-repeat -108px 0; text-align:center; padding-bottom:6px;}"+
        ".pop_qd li a:hover,.pop_qd li a.on{ text-decoration:none;background:url(" + imgSiteUrl + "pdpop_ico.gif) no-repeat 0 0;}"+
        ".qd_ht li{ padding:5px 2px; width:48px;}"+
        ".qd_ht li span{ width:52px; text-align:center;}"+
        ".ht_con{}"+
        ".ht_con a{color:#0097b3; display:block; padding-left:25px; font-size:13px; line-height:20px;}"+
        ".qdpop_bottom{ width:100%; height:25px; background:url(" + imgSiteUrl + "qdpop1018.gif) no-repeat  0 bottom #83D7DD;}"+
        /*日历*/
        ".rli_con{ padding:15px 5px 10px; _padding:13px 5px 10px;}"+
        ".rli_con form{ margin:0;padding0;}"+
        "table.biao{border-collapse:collapse; font-family:'Times New Roman','宋体';}"+
        ".rl_box{table-layout:fixed;word-break: break-all; word-wrap: break-word; margin:0; padding:0; }"+
        ".rl_box tbody,.rl_box td{margin:0; padding:0}"+
        ".rl_title {  height:34px;text-align:center;font-size:18px; color:#5c5c5c; line-height:34px; font-family:'微软雅黑'; font-weight:700; }"+
        ".week{color:#0097b3;font:15px '宋体';font-weight:bold;text-align:center;vertical-align:middle; line-height:25px; height:25px;}"+
        ".week td{ width:85px;}"+
        ".qdpop_rl{ background:url(" + imgSiteUrl + "qdpop_rl.gif) no-repeat;}"+
        ".qd_tip{ position:relative}"+
        ".qd_tip p{ padding-left:110px;}"+
        ".qd_tip span{ padding-left:35px; background:url(" + imgSiteUrl + "pdpop_ico.gif) no-repeat -212px -34px; height:40px; position:absolute; left:10px; top:3px;color:#ff8d09; font-size:30px; font-weight:bold; line-height:40px;}"+
        ".date td{font-size:9pt; overflow:hidden;cursor:default; height:36px; position:relative;color:#5c5c5c; width:85px;line-height:18px;}"+
        ".date td span{  font-size:16px; font-weight:700; display:block; /*padding:2px 0 2px 5px;*/ text-align:center;}"+
        ".date td.td_gray{ color:#dadada;}"+
        ".date td.td_qd{ background:url(" + imgSiteUrl + "pdpop_ico.gif) no-repeat -38px -76px;}"+
        ".date td.td_qdon{ background:url(" + imgSiteUrl + "pdpop_ico.gif) no-repeat -103px -74px; color:#FF8D09}"+
        ".doing em{ display:inline-block; width:8px; height:8px;background:url(" + imgSiteUrl + "pdpop_ico.gif) no-repeat; float:right; margin-right:2px;}"+
        ".doing em.do_1{ background-position:-5px -109px;margin-right:4px;}           /*紫色*/"+
        ".doing em.do_2{ background-position:-53px -109px;margin-right:4px;}          /*褐色*/"+
        ".doing em.do_3{ background-position:-17px -109px;margin-right:4px;}          /*红色*/"+
        ".doing em.do_4{ background-position:-65px -109px;margin-right:4px;}          /*蓝色*/"+
        ".doing em.do_5{ background-position:-29px -109px;margin-right:4px;}          /*绿色*/"+
        ".doing em.do_6{ background-position:-77px -109px;margin-right:4px;}          /*粉红*/"+
        ".doing em.do_7{ background-position:-41px -109px;margin-right:4px;}          /*黑色*/"+
        ".doing em.do_8{ background-position:-89px -109px;margin-right:4px;}          /*黄色*/"+

        "/*活动介绍*/"+
        ".acts{ padding:0 6px;} "+
        ".acts ul{ margin:0; padding:0} "+
        ".acts li{ float:left; margin:0 10px 5px 0; width:215px; color:#666; padding:0;}"+
        ".acts  span{ height:17px; width:80px; padding:3px; line-height:17px; display:inline-block; color:#fff; text-align:center; margin-right:5px; }"+
        ".acts span.purple{background:#6E2BA1;}"+
        ".acts span.brown{background:#9D7070;}"+
        ".acts span.red{background:#E61A1A;}"+
        ".acts span.blue{background:#4ECBDC;}"+
        ".acts span.green{background:#4DF15A;}"+
        ".acts span.pink{background:#F7AFF7;}"+
        ".acts span.dark{background:#483434;}"+
        ".acts span.yellow{background:#B3B34D;}"+
        ".acts_con li{ width:450px;}"+
        ".logbg{ width:314px; font-size:12px; color:#636363; position:absolute; right:-25px; top:37px;}"+
        "#logbg_ing{position: absolute; left: 5px; background:#FFF; width: 304px; line-height: 160px; height: 168px; _height:98px; _padding-top:70px; top: 14px; display:none}"+
        ".logbg_db{background:url(" + imgSiteUrl + "logbg.gif) no-repeat -628px 0; height:20px; width:314px; overflow:hidden; clear:both;}"+
        ".logbg_hd{background:url(" + imgSiteUrl + "logbg.gif) no-repeat 0 0; padding:20px 0 0; overflow:hidden;}"+
        ".logbg_fd{background:url(" + imgSiteUrl + "logbg.gif) repeat-y -314px 0; padding:0 5px; width:304px;}"+
        " .ltab{padding:0 10px;}"+
        " .ltab #_errmsg{padding:5px 0; margin:0; line-height:22px; width:284px; overflow:hidden;}"+
        " .ltab p{width:280px; text-align:left; margin:0}"+
        " .ltab .ltr1{ padding-top:10px; height:31px;}"+
        " .ltab .ltr2{ padding:10px 0 0 65px; width:215px; line-height:normal;}"+
        " .ltab .ltr3{ height:60px; padding-top:10px; position:relative; display:none }"+
        " .ltab .ltr3 img{ position:absolute; right:15px; top:0;}"+
        " .ltab .ltc1{width:55px; display:inline-block; text-align:right; margin-right:3px; line-height:31px; overflow:hidden;}"+
        " .ltab .ltc2{padding:0; margin:0;}"+
        " .ltab .l_i1,.ltab .l_i3{background:#FFF url(" + imgSiteUrl + "bg_input.gif) no-repeat; border:1px solid #dbdbdb; border-radius:2px; padding:3px; -webkit-transition: box-shadow .1s linear; -moz-transition: box-shadow .1s linear; outline:none; -webkit-outline-radius:0; width:190px; height:22px; vertical-align:top; line-height:22px;}"+
        " .ltab .l_i3{width:60px;}"+
        " .ltab .l_i1:focus,.ltab .l_i3:focus{border:1px solid #9BBBDA; box-shadow:0 0 5px rgba(110, 189, 222, 0.5); -webkit-box-shadow:0 0 5px rgba(110, 189, 222, 0.5);}"+
        " .ltab .l_i2{vertical-align:middle; margin:0 5px 0 0;}"+
        " .ltab .l_b1{background:url(" + imgSiteUrl + "log.gif) no-repeat 0 -61px; height:30px; line-height:30px; text-align:center; padding:0 0 0 3px; display:inline-block; color:#FFF; text-decoration:none;}"+
        " .ltab .l_b1 input{background:url(" + imgSiteUrl + "log.gif) no-repeat right -31px; padding:0 15px 0 12px; display:block; text-decoration:none; border:none; display:inline-block; height:30px; line-height:30px; cursor:pointer; color:#FFF;}"+
        " .ltab .l_b1:hover{color:#FFF; text-decoration:none;}"+
        " .ltab .l_b2{display:inline-block; height:30px; line-height:30px; padding:0 0 0 10px; color:#949494; text-decoration:none;}"+
        " .ltab .l_b2:hover{color:#333;}"+
        "#verify_img{cursor:pointer;}"+
        "#tinybox {position:absolute; display:none; font-size:12px; background:#FFF url(" + imgSiteUrl + "preload.gif) no-repeat 50% 50%; border:1px solid #D5D5D5; z-index:10002}"+
        "#tinymask {position:absolute; display:none; top:0; left:0; height:100%; width:100%; background:#000; z-index:10001}"+
        "#tinycontent {background:#fff}"+
        "#tinybox.tinynobg{ background-color:transparent; border:none;}"+
        "#tinybox.tinynobg #tinycontent{ background-color:transparent;}"+
        "#guideBox{ width:433px; height:298px; background:url(" + imgSiteUrl + "guidebox.gif) no-repeat left top}"+
        "#guideBox .guideTit{width:290px; height:40px; padding:104px 23px 8px 120px; *padding:104px 50px 8px 120px; line-height:20px; text-align:left; color:#242424; font-size:14px; font-weight:bold}"+
        "#guideBox .guideCon{width:330px; height:93px; padding-left:100px; overflow:auto; text-align:left; line-height:24px; cursor:default}"+
        "#guideBox .guideCon .oldname{color:#777; font-size:13px}"+
        "#guideBox .guideCon .newname{color:#242424; font-size:13px; line-height:24px}"+
        "#guideBox .guideCon .newname input{ border:1px solid #9ACCFF; font-size:13px; height:22px; line-height:22px}"+
        "#guideBox .guideCon .tips{font-size:12px; line-height:16px; padding:3px 20px 0 70px; color:#F00;}"+
        "#guideBox .guidebtn{ display:inline-block; width:86px; height:33px; line-height:33px; background:url(" + imgSiteUrl + "alertbtn.jpg) no-repeat left top; border:none; color:#fff; font-size:14px; font-weight:bold; text-decoration:none}"+
        "#guideBox .guidebtn:hover{ color:#FF0;}"+
        /*affiche*/
        ".public_top_affiche_full_bg{height:31px;background:#ececec;text-align:center;}"+
        ".public_top_affiche_full_bg div{height:31px;background:#ececec;width:1004px;overflow:hidden;font-size:13px;font-weight:bold;line-height:31px;color:#f64100;margin:0 auto;}"+
        ".public_top_affiche_full_bg div a{text-decoration:none;}"+
        ".public_top_affiche_full_bg div a:hover{text-decoration:underline;}"+
        ".public_top_affiche_full_bg a{color:#f64100;}"

    );
    if(type == "black"){
        document.write(
            ".public_menu_game a{color:#969696; text-decoration:none;}"+
            ".public_menu_game a.hover,.public_menu_game a:hover{color:#0AA9D6; text-decoration:none; background-color:#1a1a1a;}"+
            ".public_menu_game dd{border-right:1px solid #1a1a1a;}"+
            ".public_menu_game_col4 dd{border-right:none;}"+
            ".log_menu,.menu_task{background-color:#303030; border-color:#1a1a1a; color:#979797;}"+
            ".log_menu .menu_optb{background-color:#242426;}"+

            ".log_menu .menu_btm{border-top:1px solid #202020;}"+
            ".menu_task .task_hd{border-bottom:1px solid #202020;}"+
            ".menu_task .task_li div{border-bottom:1px solid #414141;}"+
            "#qdpop{ background:url(" + imgSiteUrl + "qdpop_b.gif) no-repeat scroll 0 0 transparent; color:#969696}"+
            ".qd_face li.on{background-color:#5D5D5D;}"+
            ".qd_face li.on span{color:#FFF;}"+
            ".qd_myword{border:1px solid #0a0a0a; background:#5D5D5D;}"+
            "</style>"
        );
    }else{
        document.write("</style>");
    }
    document.write(
        '<div style="display:none"><img src="'+imgSiteUrl+siteImg+'" id="imglogoff" />'+
        '<img src="'+imgSiteUrl+'logbg.gif" />'+
        '<img src="'+imgSiteUrl+'log.gif" />'+
        '<img src="'+imgSiteUrl+'bg_input.gif" />'+
        '<img src="'+imgSiteUrl+'preload.gif" />'+
        '<img src="'+imgSiteUrl+'qdpop0629.gif" />'+
        '<img src="'+imgSiteUrl+'icon_new.gif" />'+
        '<input type="hidden" id="topface" value="[f=1]" />'+
        '<img src="'+imgSiteUrl+'menubg.gif" /></div>'+
        '<div class="public_top_full_bg" id="public_top">'+
        ' <div class="public_top_box">'+
        '     <div class="public_top_logo">'+
        '         <a href="'+logoHref+'">'+logoTitle+'</a>'+
        '        </div>'+
        '        <ul class="public_top_menu">'+
        '            <li class="public_top_menu_s" onmouseover="show_public_top(this);" onmouseout="hide_public_top(this);"><a class="public_top_menu_t" href="#">99游戏大全</a>');
    document.write(
        '<div class="public_menu_game_hd" style="display:none;">'+
        '   <div class="public_menu_game_dt">'+
        '   <dl class="public_menu_game_col1"><dt>网络游戏</dt></dl>'+
        '   <dl class="public_menu_game_col3"><dt>手机游戏</dt></dl>'+
        '   <dl class="public_menu_game_col4" style="'+hbqStyle+'"><dt>游戏助手</dt></dl>'+
        '   </div>'+
        ' <div class="public_menu_game_fd">'+
        '   <div class="public_menu_game">'+
        ' <dl class="public_menu_game_col1">'+
        '   <dd>'+
        '<a href="https://my.99.com/" target="'+target+'" title="魔域"><span class="ico_game ico_my"></span>魔域<span style="color:#e60012">(山海异界：序曲 7月25日公测)<img src="https://img6.99.com/news/images/topmenu/0620/hot.gif" style="position:absolute" border="0"></span></a>'+
        '<a href="https://moba.99.com/" target="'+target+'" title="英魂之刃"><span class="ico_game ico_cos"></span>英魂之刃<span style="color:#e60012">(热血对战公测)<img src="https://img6.99.com/news/images/topmenu/0620/hot.gif" style="position:absolute" border="0"></span></a>'+
        '<a href="https://zf.99.com/" target="'+target+'" title="征服"><span class="ico_game ico_zf"></span>征服<span style="color:#e60012">(箭傲苍穹 7月8日公测)<img src="https://img6.99.com/news/images/topmenu/0620/hot.gif" style="position:absolute" border="0"></span></a>'+
        '<a href="https://jz.99.com/" target="'+target+'" title="机战"><span class="ico_game ico_jz"></span>机战<span style="color:#EB6001">(战域星盟公测)</span></a>'+
        '<a href="https://kx.99.com/" target="'+target+'" title="开心"><span class="ico_game ico_kx"></span>开心<span style="color:#ff002a;">(十年之约开心有你)</span></a>'+
        '   </dd>'+
        ' </dl>'+
        ' <dl class="public_menu_game_col3">'+
        '   <dd>'+
        '<a href="https://mykd.99.com/" target="'+target+'" title="魔域口袋版"><span class="ico_game ico_mykd"></span>魔域口袋版(资料片公测 上线领新装)<img src="https://img6.99.com/news/images/topmenu/0620/hot.gif" style="position:absolute" border="0"></a>'+
        '<a href="https://ht.my.99.com/" target="'+target+'" title="《魔域》互通版"><span class="ico_game ico_zsmy"></span>魔域互通版<span style="color:#e60012">(19.1M极速下载器上线)<img src="https://img6.99.com/news/images/topmenu/0620/hot.gif" style="position:absolute" border="0"></span></a>'+
        '<a href="https://yhkd.99.com/" target="'+target+'" title="英魂口袋版"><span class="ico_game ico_yhkd"></span>英魂口袋版<img src="https://img6.99.com/news/images/topmenu/0620/hot.gif" style="position:absolute" border="0"></a>'+
        '<a href="https://yhzl.99.com/" target="'+target+'" title="英魂之刃战略版" ><span class="ico_game ico_yhzl"></span>英魂之刃战略版</a>'+
        '<a href="https://kdzf.99.com/" target="'+target+'" title="口袋征服"><span class="ico_game ico_kdzf"></span>口袋征服<span style="color:#e60012">(公测)</span></a>'+
        '   </dd>'+
        ' </dl>'+
        ' <dl class="public_menu_game_col4" style="'+hbqStyle+'">'+
        '   <dd>'+
        '<a href="http://aq.99.com/" target="'+target+'" title="安全中心">安全中心</a>'+
        '<a href="https://bbs.99.com/" target="'+target+'" title="游戏论坛">游戏论坛</a>'+
        '<a href="http://ekey.99.com/" target="'+target+'" title="安全令牌">安全令牌</a>'+
        '<a href="https://down.99.com/" target="'+target+'" title="游戏下载">游戏下载</a>'+
        '<a href="https://jf.99.com/" target="'+target+'" title="包子兑换">包子兑换</a>'+
        '<a href="https://tu.99.com/" target="'+target+'" title="游戏截图">游戏截图</a>'+
        '   </dd>'+
        ' </dl>'+
        '<div style="clear:both;"></div>'+
        '   </div>'+
        ' </div>'+
        '</div>'+
        '<\/li>'+
        '<\/ul>'+
        '        <div class="public_top_skin" style="background:'+skinImg+' background-repeat:no-repeat;"><div>'+skinHtml+'</div></div>'+
        '        <ul class="public_top_link" style="'+hbqStyle+'">'+
        '            <li id="user_login"><a onclick="showlogbox()" href="javascript:;" class="public_top_link_log" style="'+btnStyle+' ">登录99社区</a>'+
        '<div class="logbg" id="logbox" style="display:none;">'+
        '<div id="logbg_ing"></div>'+
        ' <div class="logbg_hd">'+
        '   <div class="logbg_fd">'+
        ' <div class="ltab">'+
        '   <p id="_errmsg" style="display:none;text-align:center;font-weight:bold;color:red;">用户名错误</p>' +
        '   <p class="ltr1">'+
        '     <span class="ltc1">99通行证:</span>'+
        '     <span class="ltc2"><input class="l_i1" id="_username" type="text" /></span>'+
        '   </p>'+
        '   <p class="ltr1">'+
        '     <span class="ltc1">密码:</span>'+
        '     <span class="ltc2"><input class="l_i1" id="_password" type="password" /></span>'+
        '   </p>'+
        '   <p class="ltr3" id="verifycode_box">'+
        '     <span class="ltc1">验证码:</span>'+
        '     <span class="ltc2"><input class="l_i3" id="_verifycode" type="text" maxlength="4" /><img id="verify_img" onclick="showCheckCode();" title="点击更换验证码" /></span>'+
        '   </p>'+
        '   <p class="ltr2">'+
        '     <input class="l_i2" type="checkbox" id="_remember" /><label for="remember">一个月内免登录</label>'+
        '   </p>'+
        '   <p class="ltr2">'+
        '       <span class="l_b1"><input type="button" value="登 录" onclick="loginCheck();" /></span> <a class="l_b2" href="https://aq.99.com/NDUser_GetPassWordCenter.aspx" target="_blank" rel="noopener">忘记密码</a>'+
        '   </p>'+
        ' </div>'+

        '   </div>'+
        ' </div>'+
        '<div class="logbg_db"> </div></div>'+
        '            </li>'+
        '      <li class="public_top_link_loged" id="user_area" onmouseover="show_log_menu()" onmouseout="hide_log_menu()" style="display:none;">'+
        '       <div style="'+btnStyle+' ">'+
        '       <span class="rarr"></span><span class="userhead"><img id="uh01" src="https://img6.99.com/myreg91/center/face/small_nosex.gif" width="20" height="20" /></span><a class="usercount" href="javascript:;">我的帐号</a>'+
        '             <span class="log_menu_qdmsg" id="qdtip" style="display:none;">'+
        '         每日签到得10积分<br />连续2天获得20积分<br />连续3天获得40积分<br />连续4天获得70积分<br />5天以上获得120积分<br />月满勤额外奖励6000积分'+
        '       </span>'+
        '       </div>'+
        '             <div class="log_menu" id="log_menu" style="display:none;">'+
        '       <div class="menu_head"><a id="my91url1" href="https://t.99.com//member/UserSettings/eavatar"><img id="uh02" src="https://img6.99.com/myreg91/center/face/small_nosex.gif" border="0" width="80" height="80" /></a></div>'+
        '       <div class="menu_opt">'+
        '         <div class="menu_opt0"><a id="my91url2" href="https://t.99.com/"><strong id="user_nickname"></strong></a></div>'+
        '         <div class="menu_opt1">'+
        '               <a href="https://t.99.com/member/UserSettings/">帐号设置</a>'+
        '               <a href="https://t.99.com/article/view/uid/448025230/arid/10376">签到问题反馈</a>'+
        '         </div>'+
        '         <div class="menu_opt2">'+
        '                 <span class="menu_jf"><em></em><a href="https://rw.99.com/?controller=credits" target="_blank" rel="noopener"><span id="menu_jifen" title="查询积分记录"></span></a></span>'+
        '                 <a class="menu_btnqd" id="sign_btn" onclick="showqd() " onmouseover="document.getElementById(\'qdtip\').style.display=\'block\'" onmouseout="document.getElementById(\'qdtip\').style.display=\'none\'" href="javascript:;" title="点击签到">签 到</a>'+
        '                 <a class="menu_btnqd_disable" id="sign_btn_disable" style="display:none" onmouseover="document.getElementById(\'qdtip\').style.display=\'block\'" onmouseout="document.getElementById(\'qdtip\').style.display=\'none\'" href="https://t.99.com" rel="noopener" target="_blank" title="查看我的签到">今日已签到</a>'+
        '                 <p class="menu_qdinfo" id="continue_sign_count"></p>'+
        '         </div>'+
        '             <\/div>'+
        '       <div style="clear:both;"></div>'+
        '             <div class="menu_btm"><a href="https://t.99.com/logout/" class="top_logout" onclick="return ajaxLogOut();">退出</a></div></div>'+
        '            </li>'+
        '            <li><a id="game_regurl" target="'+target+'" href="https://reg.99.com/NDUser_Register_New.aspx" rel="noopener">注册</a></li>'+
        '            <li class="pipe" style="'+czStyle+'"><a target="_blank" target="'+target+'" href="https://shop.99.com/" rel="noopener">充值</a></li>'+
        '            <li class="pipe"><a href="https://gm.99.com/" target="'+target+'" rel="noopener">客服</a></li>'+
        '        </ul>'+
        '    </div>'+
        '</div>'+
        '<div class="public_top_affiche_full_bg" style="'+afficheStyle+'"><div>'+affiche+'</div></div>'
    );
    var publicTop=topGetE("public_top");
    var a=publicTop.getElementsByTagName("a");
    var aLen=a.length;
    for(var i=0;i<aLen;i++){
        var title=a[i].innerHTML;
        var layIndex=title.indexOf("<");
        if(layIndex>=0){
            title=title.substring(0,layIndex);
        }
        a[i].title=title;
    }
    if(type != "shop" & type != "aq"){
        showUserInfo();
        //showTaskList();
    }
}
//topMenu--end



//添加收藏
function myAddPanel(homeName, homePage){

    if((typeof window.sidebar == 'object') && (typeof window.sidebar.addPanel == 'function'))//Gecko
        window.sidebar.addPanel(homeName,homePage,"");
    else//IE
        window.external.AddFavorite(homePage,homeName);
}

//BUG收集
function win_open_bug(errType,width,height,siteUrl){
    var err = null;
    var url = (siteUrl != "" && siteUrl != null) ? encodeURIComponent(siteUrl) : encodeURIComponent(document.location.toString());

    try{errType = (errType != null) ? parseInt(errType) : 1;}catch(err){errType = 1;}
    try{width = (width != null) ? parseInt(width) : 800;}catch(err){width = 800;}
    try{height = (height != null) ? parseInt(height) : 720;}catch(err){height = 720;}
    window.open("https://zc.99.com/?controller=user&action=showaddstep1&i_url="+url,"errPage","fullscreen=no,channelmode=no,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=" + width.toString() + ",height=" + height.toString());
}

//处理PNG
var arVersion = navigator.appVersion.split("MSIE");
var version = parseFloat(arVersion[1]);
function transPNGPic(myImage)
{
    if ((version >= 5.5) && (version < 7) && (document.body.filters))
    {
        var imgID = (myImage.id) ? "id='" + myImage.id + "' " : "";
        var imgClass = (myImage.className) ? "class='" + myImage.className + "' " : "";
        var imgTitle = (myImage.title) ?
            "title='" + myImage.title   + "' " : "title='" + myImage.alt + "' ";
        var imgStyle = "display:inline-block;" + myImage.style.cssText;
        var strNewHTML = "<span " + imgID + imgClass + imgTitle
            + " style=\"" + "cursor:hand;width:" + myImage.width
            + "px; height:" + myImage.height
            + "px;" + imgStyle + ";"
            + "filter:progid:DXImageTransform.Microsoft.AlphaImageLoader"
            + "(src=\'" + myImage.src + "\', sizingMethod='scale');\"></span>";
        myImage.outerHTML = strNewHTML;
    }
}

//站点枚举
var site = {
    www:0,
    news:1,
    bbs:2,
    mysy2:4,
    yhkd:5,
    yhwz:10,
    tmz:11,
    xy:12,
    hl:13,
    ty:14,
    kx:15,
    zf:16,
    jz:17,
    my:18,
    ty:19,
    yhslg:20,
    aoe:39,
    cos1:40,
    wxy:44,
    hbq:45,
    fkbl:47,
    mykd:49,
    kdzf:50
};

//统一底部
function publicBottom(siteType,tbWidth,target,logoType){
    /* 参数说明
    logoType：网龙LOGO的样式，0为黑色，1为白色
    */

    var bmContext = null;
    var agepic = null;
    target = (target == "_blank" || target == "_self" || target == "_top" || target == "_parent") ? target : "_blank";
    logoType = (logoType == 0 || logoType == 1) ? logoType : 0;
    if( tbWidth <1002 ){
        tbWidth = 1050;
    }

    /* 统一参数 */

    var about91 = "<a target=\"" + target + "\" href=\"https://www.99.com/about/\" rel=\"noopener\">关于99.COM</a>";
    var badm = "<a target=\"" + target + "\" href=\"http://www.beian.gov.cn/portal/registerSystemInfo?recordcode=35010002000102\" style=\"display:inline-block;text-decoration:none;\" rel=\"noopener\"><img src=\"https://img9.99.com/news/images/topmenu/0620/ghs.png\" style=\"float:left;\" alt=\"闽公网安备\" width=\"20\" height=\"20\" />闽公网安备 35010002000102号</a>";

    var ndTopMenuYear = new Date().getFullYear();
    var ndlogo = "https://img6.99.com/dsn/img/2015/ndlogo.png";
    var txt = "健康游戏忠告：抵制不良游戏，拒绝盗版游戏，注意自我保护，谨防受骗上当。适度游戏益脑，沉迷游戏伤身，合理安排时间，享受健康生活。";
    var tqc = "福建省天晴互动娱乐有限公司";
    var ndqc = "<a target=\"" + target + "\" href=\"http://www.nd.com.cn\" rel=\"noopener\">福建网龙计算机网络信息技术有限公司</a>";
    var ndqc1 = "<a target=\"" + target + "\" href=\"http://www.nd.com.cn\" rel=\"noopener\">著作权人&出版服务单位&运营方：福建网龙计算机网络信息技术有限公司</a>";
    var webgamebf = "<a target=\"" + target + "\" href=\"https://www.99.com/about/gamerule.shtml\" rel=\"noopener\">《网络游戏管理暂行办法》</a>";
    var webgd = "<a target=\"" + target + "\" href=\"https://www.99.com/about/culturerule.shtml\" rel=\"noopener\">《互联网文化管理暂行规定》</a>";
    var twelve = "本游戏适合12岁以上的玩家进入";
    var fifteen = "本游戏适合15岁以上的玩家进入";
    var sixteen = "本游戏适合16岁以上的玩家进入";
    var eighteen = "本游戏适合18岁以上的玩家进入";
    var eighteenimg = "<img src=\"https://img6.99.com/news/images/topmenu/0620/eigtheen.png\" border=\"0\" width=\"50\" height=\"50\" alt=\"本游戏适合18岁以上的玩家进入\" >";
    var twelveimg = "<img src=\"https://img8.99.com/news/images/topmenu/0620/twelve.png\" border=\"0\" width=\"50\" height=\"50\" alt=\"本游戏适合12岁以上的玩家进入\" >";

    var contact = "<a target=\"" + target + "\" href=\"https://www.99.com/about/contact.shtml\" rel=\"noopener\">联系我们</a>";
    var invite = "<a target=\"" + target + "\" href=\"http://hr.nd.com.cn/\" rel=\"noopener\">高薪诚聘</a>";

    var reg = "<a target=\"" + target + "\" href=\"https://reg.99.com/\" rel=\"noopener\">会员注册</a>";
    var question = "<a target=\"" + target + "\" href=\"https://www.99.com/about/sitemap.shtml\" rel=\"noopener\">站点地图</a>";
    var report = "<a target=\"" + target + "\" href=\"http://icac.99.com/\" rel=\"noopener\">举报</a>";

    var copyright = "Copyright 1999-" + ndTopMenuYear + " @ <a target=\"" + target + "\" href=\"https://www.99.com/\" rel=\"noopener\">99.com</a> All rights reserved.";
    var copyright2 = " 2000-" + ndTopMenuYear + " @ <a target=\"" + target + "\" href=\"https://www.99.com/\" rel=\"noopener\">99.com</a> All rights reserved.";
    var copyright3 = "Copyright &copy; 2016-" + ndTopMenuYear + " 99.com All rights reserved.";

    var cbxk = "<a target=\"" + target + "\" href=\"https://www.99.com/about/interlicense.shtml\" rel=\"noopener\">网络出版服务许可证</a>";
    var increment = "<a target=\"" + target + "\" href=\"https://www.99.com/about/licence.shtml\" rel=\"noopener\">增值电信业务经营许可证闽B1.B2-20100001</a>";
    var tqincrement = "<a target=\"" + target + "\" href=\"http://www.tqinteractive.com/about/license.shtml\" rel=\"noopener\">增值电信业务经营许可证闽B2-20170081</a>";
    var netArticle = "<a target=\"" + target + "\" href=\"http://sq.ccm.gov.cn:80/ccnt/sczr/service/business/emark/toDetail/D50A9C605C964B5FAD20C1CD34885C5F\" rel=\"noopener\">闽网文(2017)8315-214号</a>";
    var tqnetArticle = "<a target=\"" + target + "\" href=\"http://www.tqinteractive.com/about/telecom.shtml\" rel=\"noopener\">闽网文(2016)5715-105号</a>";
    var icpba = "<a target=\"" + target + "\" href=\"http://beian.miit.gov.cn\" rel=\"noopener\"><img src=\"https://img8.99.com/news/images/topmenu/icp2.png\" width=\"34\" height=\"50\" alt=\"闽ICP备B2-20050038\" border=\"0\" title=\"闽ICP备B2-20050038\" \/><br>闽ICP备<br>B2-20050038</a>";
    var vip = "重要客户呼叫中心号码：0591-88085999";

    /* 各官网额外参数 */
    var top = "<a href=\"" + window.location.href + "#Top\" target=\"_self\">Top</a>";

    var tmzphone = "<a target=\"" + target + "\" href=\"https://gm.99.com/\">客服中心</a>";
    var tmzmail = "客服邮箱：<a target=\"" + target + "\" href=\"mailto:tmzgm@tqdigital.com\">tmzgm@tqdigital.com</a>";
    var tmz = "《<a target=\"" + target + "\" href=\"https://tmz.99.com/\">投名状Online</a>》中国官方站";
    var tmzba = "<a target=\"" + target + "\" href=\"https://tmz.99.com/guide/faq/whb.shtml\">文网备字（2008）04号</a>";
    var tmzwh = "新出音管[2008]036号";
    var tmzisbn = "ISBN978-7-900419-34-7/TP·30";

    var kxphone = "客服电话：0591-87085708";
    var kxvip = "VIP客户电话：0591-88085999-2";
    var kx = "《<a target=\"" + target + "\" href=\"https://kx.99.com/\">开心</a>》中国官方站";
    var kxba = "<a target=\"" + target + "\" href=\"https://kx.99.com/guide/faq/whb.shtml\">文网备字（2008）12号</a>";
    var kxisbn = "ISBN978-7-900419-33-0/TP·29";
    var kxwh = "新出音管[2008]036号";
    var kxyszc = "<a target=\"" + target + "\" href=\"https://kx.99.com/content/2020-12-29/yszc.shtml\">隐私政策</a>";
    var kxkidyszc = "<a target=\"" + target + "\" href=\"https://kx.99.com/content/2020-12-29/kid-yszc.shtml\">未成年人（含儿童）隐私政策</a>";

    var hlmail = "客服邮箱：<a target=\"" + target + "\" href=\"mailto:service@tqdigital.com\">service@tqdigital.com</a>";
    var hl = "《<a target=\"" + target + "\" href=\"https://hl.99.com/\">幻灵</a>》中国官方站";
    var hlba = "<a target=\"" + target + "\" href=\"https://hlyx.99.com/guide/faq/whb.shtml\">文网备字（2008）16号</a>";
    var hlisbn = "ISBN7-900351-77-9/TP·63";
    var hlwh = "新出音管[2007]138号";

    var zfphone = "客服电话：0591-87085777";
    var zfmail = "客服邮箱：<a target=\"" + target + "\" href=\"mailto:service@tqzf.com\">service@tqzf.com</a>";
    var zf = "《<a target=\"" + target + "\" href=\"https://zf.99.com/\">征服</a>》中国官方站";
    var zfba = "<a target=\"" + target + "\" href=\"https://zf.99.com/guide/faq/whb.shtml\">文网备字（2007）19号</a>";
    var zfisbn = "ISBN7-900351-70-1/TP·57";
    var zfwh = "新出音管[2007]138号";

    var kdzfphone = "客服电话：0591-87085761";
    var kdzf = "《<a target=\"" + target + "\" href=\"https://kdzf.99.com/\">口袋征服</a>》中国官方站";
    var kdzfisbn = "ISBN978-7-7979-8379-2";
    var kdzfwh = "新广出审[2017]5095号";
    var kdzfba = "文网游备字(2016)M-RPG 6973号";
    var kdzfyhxx = "<a target=\"" + target + "\" href=\"https://kdzf.99.com/news/08072019/yhxx.shtml\">用户协议</a>";
    var kdzfyxzc = "<a target=\"" + target + "\" href=\"https://kdzf.99.com/news/08072019/yszc.shtml\">隐私政策</a>";
    var kdzfkidyszc = "<a target=\"" + target + "\" href=\"https://kdzf.99.com/news/08072019/childyszc.shtml\">未成年人（含儿童）隐私政策</a>";

    var jzphone = "客服电话：0591-87085756";
    var jzmail = "客服邮箱：<a target=\"" + target + "\" href=\"mailto:jzgm@tqdigital.com\">jzgm@tqdigital.com</a>";
    var jz = "《<a target=\"" + target + "\" href=\"https://jz.99.com/\">机战</a>》中国官方站";
    var jzba = "<a target=\"" + target + "\" href=\"https://jz.99.com/guide/faq/whb.shtml\">文网备字（2007）06号</a>";
    var jzisbn = "ISBN7-900419-04-7/TP·03";
    var jzwh = "新出音管[2007]138号";

    var myphone = "客服电话：0591-87085777";
    var mymail = "客服邮箱：<a target=\"" + target + "\" href=\"mailto:tqmy@tqdigital.com\">tqmy@tqdigital.com</a>";
    var my = "《<a target=\"" + target + "\" href=\"https://my.99.com/\">魔域</a>》中国官方站";
    var myba = "<a target=\"" + target + "\" href=\"https://my.99.com/guide/faq/whb.shtml\">文网游备字〔2017〕Ｃ-RPG 0071</a>";
    agepic = eighteenimg;
    var myisbn = "ISBN7-900419-03-9/TP·02";

    var htmyisbn = "ISBN978-7-498-05628-3";
    var htmy = "《<a target=\"" + target + "\" href=\"https://ht.my.99.com/\">魔域互通版</a>》中国官方站";
    var htmyba = "闽新广【2017】848号";
    var htmyysxy = "<a target=\"" + target + "\" href=\"https://ht.my.99.com/news/03052020/220053140.shtml\">隐私协议</a>";
    var htmyyyqx = "<a target=\"" + target + "\" href=\"https://ht.my.99.com/news/01062021/224623679.shtml\">应用权限</a>";
    var htmybbh = "版本：2.5.6";
    var htmygx = "更新时间：2020-12-31";

    var xyphone = "客服中心：<a target=\"" + target + "\" href=\"https://gm.99.com/\">gm.99.com</a>";
    var xymail = "客服邮箱：<a target=\"" + target + "\" href=\"mailto:xy@tqdigital.com\">xy@tqdigital.com</a>";
    var xy = "《<a target=\"" + target + "\" href=\"https://xy.99.com/\">信仰</a>》中国官方站";
    var xyba = "<a target=\"" + target + "\" href=\"https://xy.99.com/guide/faq/whb.shtml\">文网备字（2008）15号</a>";

    var typhone = "客服电话：0591-87085777";
    var tymail = "客服邮箱：<a target=\"" + target + "\" href=\"mailto:tygm@tqdigital.com\">tygm@tqdigital.com</a>";
    var ty = "《<a target=\"" + target + "\" href=\"https://ty.99.com/\">天元</a>》中国官方站";
    var tyba = "<a target=\"" + target + "\" href=\"https://ty.99.com/guide/faq/whb.shtml\">文网游备字{2010}C-RPG023号</a>";
    var tyisbn = "ISBN978-7-89989-096-7";
    var tywh = "科技与数字[2010]322号";

    var aoe = "《<a target=\"" + target + "\" href=\"https://aoe.99.com/\">猎龙战记</a>》中国官方站";

    var cos1phone = "客服QQ：800077364";
    var cos1 = "《<a target=\"" + target + "\" href=\"https://moba.99.com/\">英魂之刃</a>》中国官方站";
    var cos1ba = "<a target=\"" + target + "\" href=\"https://newscos.99.com/guide/faq/whb.shtml\" rel=\"noopener\">文网游备字【2013】W-CSG002号</a>";
    var cos1isbn = "ISBN978-7-89989-673-0";
    var cos1wh = "新出审字[2013]655号";

    var hbq = "《<a target=\"" + target + "\" href=\"https://hbq.99.com/\">虎豹骑</a>》中国官方网站";
    var hbqba = "<a href=\"http://sq.ccm.gov.cn:80/ccnt/sczr/service/business/emark/gameNetTag/4028c08c4de794ee014e24ac947b0c93 \">文网游备字(2015)C-RPG 0547号</a>";
    var hbqisbn = "ISBN978-7-89988-489-8";
    var hbqwh = "新广出审[2015]1323号";
    var hbqzzq = "著作权人&出版服务单位：福建网龙计算机网络信息技术有限公司";
    var hbqyy = "运营方：深圳市腾讯计算机系统有限公司";

    var fkbl = "《<a target=\"" + target + "\" href=\"http://fkbl.99.com/index/\">疯狂部落</a>》中国官方网站";
    var fkblphone = "客服电话：0591-87085761";
    var fkblba = "文网游备字[2013]M-RPG027号";
    var fkblisbn = "闽新出[2013]90号";
    var fkblspwh = "新广出审[2014]612号";

    var mykd = "《<a target=\"" + target + "\" href=\"https://mykd.99.com/\">魔域口袋版</a>》中国官方网站";
    var mykdphone = "客服电话：0591-87085761";
    var mykdwh = "<a target=\"" + target + "\" href=\"http://www.gapp.gov.cn/zongshu/serviceContent2.shtml?ID=1675\">新广出审[2014]1059号</a>";
    var mykdyxgl = "<a target=\"" + target + "\" href=\"https://mykd.99.com/news/11272013/xieyi.shtml\">游戏管理协议</a>";
    var mykdba = "<a target=\"" + target + "\" href=\"http://sq.ccm.gov.cn/ccnt/sczr/service/business/emark/gameNetTag/4028c08c5f2f8ae9015f47542ef375a1\">文网游备字[2017]M-RPG 1525号</a>";
    var mykdysxy = "<a target=\"" + target + "\" href=\"https://m.mykd.99.com/news/05302019/privacy.shtml\">隐私协议</a>";
    var mykdisbn = "ISBN978-7-89988-084-5";

    var yhkdphone = "客服QQ：800808689";
    var yhkd = "《<a target=\"" + target + "\" href=\"https://yhkd.99.com/\">英魂之刃口袋版</a>》中国官方站";
    var yhkdzzq = "著作权人：福建省天晴互动娱乐有限公司";
    var yhkdndqc = "出版服务单位&运营方：福建网龙计算机网络信息技术有限公司";
    var yhkdwh = "新广出审[2016]2217号";
    var yhkdba = "文网游备字(2016)M-CSG 6750号";
    var yhkdisbn = "ISBN978-7-7979-0884-9";
    var yhkdysxy = "<a target=\"" + target + "\" href=\"http://yhkd.99.com/m/game/privacy/\">隐私协议</a>";

    var yhwz = "《<a target=\"" + target + "\" href=\"https://yhwz.99.com/\">英魂王座</a>》中国官方网站";

    var yhslg = "《<a target=\"" + target + "\" href=\"https://yhzl.99.com/\">英魂之刃战略版</a>》中国官方站";
    var yhslgmail = "客服邮箱：<a target=\"" + target + "\" href=\"mailto:yhzrservice@netdragon.com\">yhzrservice@netdragon.com</a>";
    var yhslgwh = "国新出审[2019]2732号";
    var yhslgisbn = "ISBN978-7-498-06823-1";
    var yhslgzzq = "著作权人&运营方：福建省天晴互动娱乐有限公司";
    var yhslgndqc = "出版服务单位：福建网龙计算机网络信息技术有限公司";
    var yhslgysxy = "<a target=\"" + target + "\" href=\"https://yhzl.99.com/m/game/privacy/\">隐私协议</a>";

    var mysy2 = "《<a target=\"" + target + "\" href=\"https://mzj.99.com/\">魔战纪</a>》中国官方网站";
    var mysy2wh = "国新出审[2020]858号";
    var mysy2isbn = "ISBN978-7-498-07617-5";
    var mysy2ndqc = "出版单位&运营单位：福建网龙计算机网络信息技术有限公司";

    var _s_="┊";

    /*如果为魔域口袋版*/
    if(siteType==site.mykd){
        document.writeln("<table width=\""+tbWidth+"\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" id=\"public_bottom_table\" class=\"public_bm_table\"><tr class=\"public_bm_tr\">"+
            "<td class=\"public_bm_td2\" id=\"public_bottom_context\"> <\/td>"+
            "<td width=\"70\" align=\"center\" class=\"public_bm_td3\" rowspan=\"2\"><a target=\"" + target + "\" href=\"http:\/\/sq.ccm.gov.cn\/ccnt\/sczr\/service\/business\/emark\/gameNetTag\/4028c08c5f2f8ae9015f47542ef375a1\" target=\"_blank\"><img src=\"https:\/\/img5.99.com\/news\/2014\/08\/18\/gamerfid-v1.png\" width=\"45\" border=\"0\" onload=\"transPNGPic(this);\" \/><\/a><\/td>"+
            "<td width=\"90\" align=\"center\" class=\"public_bm_td3\" rowspan=\"2\">"+icpba+"<\/td>"+
            "<td width=\"65\" align=\"center\" class=\"public_bm_td4\" rowspan=\"2\"><a target=\"" + target + "\" href=\"http:\/\/www.cyberpolice.cn\/wfjb\/\" target=\"_blank\" rel=\"noopener\"><img src=\"https:\/\/img5.99.com\/news\/images\/topmenu\/netpolice2.png\" width=\"50\" height=\"51\" alt=\"报警岗亭\" title=\"报警岗亭\" border=\"0\" onload=\"transPNGPic(this);\" \/><\/a><\/td>"+
            "<td width=\"65\" align=\"center\" class=\"public_bm_td4\" rowspan=\"2\"><a target=\"" + target + "\" href=\"http:\/\/www.99.com\/gm91\/zhuanti\/jianhu\/index.shtml\" target=\"_blank\" rel=\"noopener\"><img src=\"https:\/\/img5.99.com\/news\/images\/topmenu\/1127\/jianhu2.gif\" alt=\"家长监护工程\" title=\"家长监护工程\" border=\"0\" onload=\"transPNGPic(this);\" \/><\/a><\/td>"+
            "<td width=\"60\" align=\"center\" id=\"public_bm_td5\" class=\"public_bm_td4\" rowspan=\"2\"><\/td>"+
            "<\/tr><\/table>");
        var bmContext =  mykd+_s_+mykdphone+_s_+eighteen+_s_+mykdysxy+_s_+mykdyxgl+"<br\/>"+
            copyright2+_s_+mykdba+_s_+mykdwh+"<br\/>"+
            ndqc1+_s_+mykdisbn+"<br\/>"+
            increment+_s_+badm;
        agepic = eighteenimg;
        topGetE("public_bottom_context").innerHTML = bmContext +"<br\/>"+ txt;
        topGetE("public_bm_td5").innerHTML = agepic ;
        return;
    }

    document.writeln("<table width=\""+tbWidth+"\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" id=\"public_bottom_table\" class=\"public_bm_table\"><tr class=\"public_bm_tr\">"+
        "<td class=\"public_bm_td2\" id=\"public_bottom_context\"> <\/td>"+
        "<td width=\"100\" align=\"center\" class=\"public_bm_td3\" rowspan=\"2\">"+icpba+"<\/td>"+
        "<td width=\"65\" align=\"center\" class=\"public_bm_td4\" rowspan=\"2\"><a target=\"" + target + "\" rel=\"noopener\" href=\"http:\/\/www.cyberpolice.cn\/wfjb\/\" target=\"_blank\"><img src=\"https:\/\/img5.99.com\/news\/images\/topmenu\/netpolice2.png\" width=\"50\" height=\"51\" alt=\"报警岗亭\" title=\"报警岗亭\" border=\"0\" onload=\"transPNGPic(this);\" \/><\/a><\/td>"+
        "<td width=\"60\" align=\"center\" class=\"public_bm_td4\" rowspan=\"2\"><a target=\"" + target + "\" rel=\"noopener\" href=\"http:\/\/www.99.com\/gm91\/zhuanti\/jianhu\/index.shtml\" target=\"_blank\"><img src=\"https:\/\/img5.99.com\/news\/images\/topmenu\/1127\/jianhu2.gif\" width=\"48\" height=\"48\" alt=\"家长监护工程\" title=\"家长监护工程\" border=\"0\" onload=\"transPNGPic(this);\" \/><\/a><\/td>"+
        "<td width=\"60\" align=\"center\" id=\"public_bm_td5\" class=\"public_bm_td4\" rowspan=\"2\"><\/td>"+
        "<\/tr><\/table>");

    //检查有没注册链接
    var regurl = topGetE("game_regurl");

    //参数组合
    var bmContext;
    switch(siteType){
        case site.bbs:
            bmContext = about91+_s_+contact+_s_+invite+_s_+question+_s_+report+_s_+ndqc+"<br\/>"+
                copyright+"<br\/>"+
                webgd+_s_+increment;
            agepic = eighteenimg ;
            break;
        case site.www:
            bmContext = about91+_s_+contact+_s_+invite+_s_+question+_s_+report+_s_+ndqc+"<br\/>"+
                copyright+_s_+cbxk+"<br\/>"+
                webgd+_s_+increment;
            agepic = eighteenimg ;
            break;
        case site.my:
            bmContext = my+_s_+myphone+_s_+vip+_s_+mymail+"<br\/>"+
                copyright2+_s_+myba+_s_+eighteen+"<br\/>"+
                ndqc1+_s_+myisbn+"<br\/>"+
                increment+_s_+badm;
            agepic = eighteenimg ;
            break;
        case site.htmy:
            bmContext = htmy+_s_+myphone+_s_+vip+_s_+mymail+"<br\/>"+
                copyright2+_s_+htmyba+_s_+eighteen+"<br\/>"+
                ndqc1+_s_+htmyisbn+_s_+htmybbh+_s_+htmygx+"<br\/>"+
                increment+_s_+badm+_s_+htmyysxy+_s_+htmyyyqx;
            agepic = eighteenimg ;
            break;
        case site.zf:
            if(regurl) regurl.href = "https://aq.99.com/NDUser_Register_New.aspx?url=http://zf.99.com&flag=zf";
            bmContext = zf+_s_+zfphone+_s_+vip+"<br\/>"+
                copyright2+_s_+zfba+_s_+zfwh+_s_+eighteen+"<br\/>"+
                ndqc1+_s_+zfisbn+"<br\/>"+
                increment+_s_+badm;
            agepic = eighteenimg ;
            break;
        case site.kdzf:
            if(regurl) regurl.href = "https://reg.99.com/NDUser_Register_New.aspx?url=http://kdzf.99.com/&flag=kdzf";
            bmContext = kdzf+_s_+kdzfphone+_s_+eighteen+_s_+kdzfyxzc+_s_+kdzfkidyszc+_s_+kdzfyhxx+"<br\/>"+
                copyright2+_s_+kdzfba+_s_+kdzfwh+"<br\/>"+
                ndqc1+_s_+kdzfisbn+"<br\/>"+
                increment+_s_+badm;
            agepic = eighteenimg ;
            break;
        case site.jz:
            if(regurl) regurl.href = "https://reg.99.com/NDUser_Register_New.aspx?url=http://jz.99.com&flag=jz";
            bmContext = jz+_s_+jzphone+_s_+vip+"<br\/>"+
                copyright2+_s_+jzba+_s_+jzwh+_s_+eighteen+"<br\/>"+
                ndqc1+_s_+jzisbn+"<br\/>"+
                increment+_s_+badm;
            agepic = eighteenimg ;
            break;
        case site.kx:
            if(regurl) regurl.href = "https://aq.99.com/NDUser_Register_New.aspx?http://kx.99.com&flag=kx";
            bmContext = kx+_s_+kxphone+_s_+kxvip+_s_+kxyszc+_s_+kxkidyszc+"<br\/>"+
                copyright2+_s_+kxba+_s_+kxwh+_s_+eighteen+"<br\/>"+
                ndqc1+_s_+kxisbn+"<br\/>"+
                increment+_s_+badm;
            agepic = eighteenimg ;
            break;
        case site.ty:
            if(regurl) regurl.href = "https://reg.99.com/NDUser_Register_New.aspx?url=http://ty.99.com&flag=ty";
            bmContext = ty+_s_+typhone+"<br\/>"+
                copyright2+_s_+tyba+_s_+tywh+_s_+eighteen+"<br\/>"+
                ndqc1+_s_+tyisbn+"<br\/>"+
                increment+_s_+badm;
            agepic = eighteenimg ;
            break;
        case site.cos1:
            if(regurl) regurl.href = "https://reg.99.com/NDUser_Register_New.aspx?url=http://moba.99.com&flag=cos";
            bmContext = cos1+_s_+cos1phone+_s_+twelve+"<br\/>"+
                copyright2+_s_+cos1ba+_s_+cos1wh+"<br\/>"+
                ndqc1+_s_+cos1isbn+"<br\/>"+
                increment+_s_+badm;
            agepic = twelveimg ;
            break;
        case site.yhkd:
            if(regurl) regurl.href = "https://reg.99.com/NDUser_Register_New.aspx?url=http://yhkd.99.com&flag=yhkd";
            bmContext = yhkd+_s_+twelve+"<br\/>"+
                yhkdba+_s_+yhkdwh+_s_+yhkdzzq+"<br\/>"+
                yhkdndqc+_s_+yhkdisbn+_s_+yhkdysxy+"<br\/>"+
                copyright3+_s_+increment+_s_+badm;
            agepic = twelveimg ;
            break;
        case site.yhslg:
            if(regurl) regurl.href = "https://reg.99.com/NDUser_Register_New.aspx?url=https://yhzl.99.com&flag=yhzl";
            bmContext = yhslg+_s_+yhslgmail+_s_+yhslgwh+_s_+twelve+"<br\/>"+
                yhslgzzq+_s_+yhslgndqc+_s_+yhslgisbn+_s_+yhslgysxy+"<br\/>"+
                copyright3+_s_+increment+_s_+badm;
            agepic = twelveimg ;
            break;
        case site.tmz:
            bmContext = tmz+_s_+tmzphone+_s_+tmzmail+_s_+eighteen+"<br\/>"+
                copyright2+_s_+tmzba+_s_+tmzwh+"<br\/>"+
                ndqc1+_s_+tmzisbn+"<br\/>"+
                increment+_s_+badm;
            agepic = eighteenimg ;
            break;
        case site.hl:
            if(regurl) regurl.href = "http://hlreg.99.com/common/signup.aspx";
            bmContext = hl+_s_+hlmail+_s_+eighteen+"<br\/>"+
                copyright2+_s_+hlba+_s_+hlwh+"<br\/>"+
                ndqc1+_s_+hlisbn+"<br\/>"+
                increment+_s_+badm;
            agepic = eighteenimg ;
            break;
        case site.hbq:
            if(regurl) regurl.href = "https://reg.99.com/NDUser_Register_New.aspx?url=https://hbq.99.com&flag=hbq";
            bmContext = hbq+_s_+eighteen+"<br\/>"+
                copyright2+_s_+hbqba+_s_+hbqwh+"<br\/>"+
                hbqzzq+_s_+hbqyy+_s_+hbqisbn+"<br\/>"+
                increment+_s_+badm;
            agepic = eighteenimg ;
            break;
        case site.yhwz:
            if(regurl) regurl.href = "https://reg.99.com/NDUser_Register_New.aspx?url=https://yhwz.99.com&flag=yhwz";
            bmContext = yhwz+_s_+tqc+"<br\/>"+
                copyright3+"<br\/>"+
                webgd+_s_+increment;
            agepic = twelveimg ;
            break;
        case site.mysy2:
            if(regurl) regurl.href = "https://reg.99.com/NDUser_Register_New.aspx?url=https://mzj.99.com&flag=mzj";
            bmContext = mysy2+_s_+eighteen+"<br\/>"+
                copyright2+_s_+mysy2wh+_s_+mysy2isbn+"<br\/>"+
                mysy2ndqc+"<br\/>"+
                increment+_s_+badm;
            agepic = eighteenimg ;
            break;
        default:
            bmContext = about91+_s_+contact+_s_+invite+_s_+question+_s_+report+_s_+ndqc+"<br\/>"+
                copyright+"<br\/>"+
                webgd+_s_+increment;
            agepic = eighteenimg ;
            break;
    }
    topGetE("public_bottom_context").innerHTML = bmContext +"<br\/>"+ txt;
    topGetE("public_bm_td5").innerHTML = agepic ;

    function newFunction() {
        return $('#public_bm_td5');
    }
}

//判断是否为空对象或空值字符串
function isEmptyStr(str){
    return (str == null) ? true : (str == "") ? true : false;
}

function win_open_advice(errType,width,height,siteUrl){
    var err = null;
    var url = (siteUrl != "" && siteUrl != null) ? encodeURIComponent(siteUrl) : encodeURIComponent(document.location.toString());
    try{errType = (errType != null) ? parseInt(errType) : 1;}catch(err){errType = 1;}
    try{width = (width != null) ? parseInt(width) : 800;}catch(err){width = 800;}
    try{height = (height != null) ? parseInt(height) : 720;}catch(err){height = 720;}
    window.open("https://zc.99.com/?controller=user&action=showaddadvicestep1&i_url="+url,"errPage","fullscreen=no,channelmode=no,toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,width=" + width.toString() + ",height=" + height.toString());
}

//======================================以下函数为新版导航登录模块使用======================================//
//md5加密算法
/*账号MD5加密*/
function top_MD5(str) { var hex_chr = "0123456789abcdef"; function rhex(num) { str = ""; for (j = 0; j <= 3; j++) str += hex_chr.charAt((num >> (j * 8 + 4)) & 0x0F) + hex_chr.charAt((num >> (j * 8)) & 0x0F); return str; } function str2blks_MD5(str) { nblk = ((str.length + 8) >> 6) + 1; blks = new Array(nblk * 16); for (i = 0; i < nblk * 16; i++) blks[i] = 0; for (i = 0; i < str.length; i++) blks[i >> 2] |= str.charCodeAt(i) << ((i % 4) * 8); blks[i >> 2] |= 0x80 << ((i % 4) * 8); blks[nblk * 16 - 2] = str.length * 8; return blks; } function add(x, y) { var lsw = (x & 0xFFFF) + (y & 0xFFFF); var msw = (x >> 16) + (y >> 16) + (lsw >> 16); return (msw << 16) | (lsw & 0xFFFF); } function rol(num, cnt) { return (num << cnt) | (num >>> (32 - cnt)); } function cmn(q, a, b, x, s, t) { return add(rol(add(add(a, q), add(x, t)), s), b); } function ff(a, b, c, d, x, s, t) { return cmn((b & c) | ((~b) & d), a, b, x, s, t); } function gg(a, b, c, d, x, s, t) { return cmn((b & d) | (c & (~d)), a, b, x, s, t); } function hh(a, b, c, d, x, s, t) { return cmn(b ^ c ^ d, a, b, x, s, t); } function ii(a, b, c, d, x, s, t) { return cmn(c ^ (b | (~d)), a, b, x, s, t); } x = str2blks_MD5(str); var a = 1732584193; var b = -271733879; var c = -1732584194; var d = 271733878; for (i = 0; i < x.length; i += 16) { var olda = a; var oldb = b; var oldc = c; var oldd = d; a = ff(a, b, c, d, x[i + 0], 7, -680876936); d = ff(d, a, b, c, x[i + 1], 12, -389564586); c = ff(c, d, a, b, x[i + 2], 17, 606105819); b = ff(b, c, d, a, x[i + 3], 22, -1044525330); a = ff(a, b, c, d, x[i + 4], 7, -176418897); d = ff(d, a, b, c, x[i + 5], 12, 1200080426); c = ff(c, d, a, b, x[i + 6], 17, -1473231341); b = ff(b, c, d, a, x[i + 7], 22, -45705983); a = ff(a, b, c, d, x[i + 8], 7, 1770035416); d = ff(d, a, b, c, x[i + 9], 12, -1958414417); c = ff(c, d, a, b, x[i + 10], 17, -42063); b = ff(b, c, d, a, x[i + 11], 22, -1990404162); a = ff(a, b, c, d, x[i + 12], 7, 1804603682); d = ff(d, a, b, c, x[i + 13], 12, -40341101); c = ff(c, d, a, b, x[i + 14], 17, -1502002290); b = ff(b, c, d, a, x[i + 15], 22, 1236535329); a = gg(a, b, c, d, x[i + 1], 5, -165796510); d = gg(d, a, b, c, x[i + 6], 9, -1069501632); c = gg(c, d, a, b, x[i + 11], 14, 643717713); b = gg(b, c, d, a, x[i + 0], 20, -373897302); a = gg(a, b, c, d, x[i + 5], 5, -701558691); d = gg(d, a, b, c, x[i + 10], 9, 38016083); c = gg(c, d, a, b, x[i + 15], 14, -660478335); b = gg(b, c, d, a, x[i + 4], 20, -405537848); a = gg(a, b, c, d, x[i + 9], 5, 568446438); d = gg(d, a, b, c, x[i + 14], 9, -1019803690); c = gg(c, d, a, b, x[i + 3], 14, -187363961); b = gg(b, c, d, a, x[i + 8], 20, 1163531501); a = gg(a, b, c, d, x[i + 13], 5, -1444681467); d = gg(d, a, b, c, x[i + 2], 9, -51403784); c = gg(c, d, a, b, x[i + 7], 14, 1735328473); b = gg(b, c, d, a, x[i + 12], 20, -1926607734); a = hh(a, b, c, d, x[i + 5], 4, -378558); d = hh(d, a, b, c, x[i + 8], 11, -2022574463); c = hh(c, d, a, b, x[i + 11], 16, 1839030562); b = hh(b, c, d, a, x[i + 14], 23, -35309556); a = hh(a, b, c, d, x[i + 1], 4, -1530992060); d = hh(d, a, b, c, x[i + 4], 11, 1272893353); c = hh(c, d, a, b, x[i + 7], 16, -155497632); b = hh(b, c, d, a, x[i + 10], 23, -1094730640); a = hh(a, b, c, d, x[i + 13], 4, 681279174); d = hh(d, a, b, c, x[i + 0], 11, -358537222); c = hh(c, d, a, b, x[i + 3], 16, -722521979); b = hh(b, c, d, a, x[i + 6], 23, 76029189); a = hh(a, b, c, d, x[i + 9], 4, -640364487); d = hh(d, a, b, c, x[i + 12], 11, -421815835); c = hh(c, d, a, b, x[i + 15], 16, 530742520); b = hh(b, c, d, a, x[i + 2], 23, -995338651); a = ii(a, b, c, d, x[i + 0], 6, -198630844); d = ii(d, a, b, c, x[i + 7], 10, 1126891415); c = ii(c, d, a, b, x[i + 14], 15, -1416354905); b = ii(b, c, d, a, x[i + 5], 21, -57434055); a = ii(a, b, c, d, x[i + 12], 6, 1700485571); d = ii(d, a, b, c, x[i + 3], 10, -1894986606); c = ii(c, d, a, b, x[i + 10], 15, -1051523); b = ii(b, c, d, a, x[i + 1], 21, -2054922799); a = ii(a, b, c, d, x[i + 8], 6, 1873313359); d = ii(d, a, b, c, x[i + 15], 10, -30611744); c = ii(c, d, a, b, x[i + 6], 15, -1560198380); b = ii(b, c, d, a, x[i + 13], 21, 1309151649); a = ii(a, b, c, d, x[i + 4], 6, -145523070); d = ii(d, a, b, c, x[i + 11], 10, -1120210379); c = ii(c, d, a, b, x[i + 2], 15, 718787259); b = ii(b, c, d, a, x[i + 9], 21, -343485551); a = add(a, olda); b = add(b, oldb); c = add(c, oldc); d = add(d, oldd); } return rhex(a) + rhex(b) + rhex(c) + rhex(d); }

function getMD5Value(data) {
    var a = data;
    var b = "\xa3\xac\xa1\xa3";
    var c = "fdjf,jkgfkl";
    var s = a + b + c;
    return top_MD5(s);
}

var logging = false;
var qdflag = false;
var todaypoint = 0;
function loginCheck(){
    if(!logging){

        var username = topGetE('_username').value;
        var passwordori = topGetE('_password').value;
        var errmsg = topGetE('_errmsg');

        var logbg_ing = topGetE('logbg_ing');

        if(username =='' || passwordori =='' ) {
            errmsg.innerHTML = "99通行证帐号或密码为空，请重输！";
            errmsg.style.display = 'block';
        }
        else {
            errmsg.style.display = 'none';
            topGetE('verifycode_box').style.display = 'none';
            logbg_ing.style.display = 'block';

            logbg_ing.innerHTML = '<img src="https://img5.99.com/news/images/topmenu/0620/preload.gif" /> 正在登录，请稍候...';
            var password = getMD5Value(topGetE('_password').value);
            var remember = topGetE('_remember').checked ? 1 : 0;
            var checkcode = topGetE('_verifycode').value;
            var host = window.location.href.split('/')[2];

            var hostArr = host.split('.');
            var hostNum = hostArr[hostArr.length-2];
            var loginUrl = "https://aq."+ hostNum +".com/AjaxAction/AC_userlogin.ashx?siteflag=200&nduseraction=login&txtUserName=" + username + "&txtPassword=" + password + "&rbl_recordinfo=" + remember + "&checkcode=" + checkcode + "&_time=" + new Date().getTime();
            logging = true;//防止重复提交
            new getjson().set(loginUrl, function(data){
                if(data.OpCode == 4){
                    //登录成功
                    document.location.reload();
                    showUserInfo(1);
                }else if(data.OpCode == 1){
                    //验证码错误
                    if(topGetE('verifycode_box').style.display != 'none'){
                        errmsg.innerHTML = "验证码错误，请重输！";
                        errmsg.style.display = 'block';
                    }
                    logbg_ing.style.display = 'none';
                    showCheckCode();
                }else{
                    switch (data.OpCode){
                        case 2: msg = "请输入99通行证帐号！"; break;
                        case 3: msg = "请输入99通行证密码！"; break;
                        case 6: msg = "帐号不存在，请检查！"; break;
                        case 7: msg = "密码错误，请检查！"; break;
                        default: msg = "系统错误，请稍候重试！";
                    }
                    errmsg.innerHTML = msg;
                    errmsg.style.display = 'block';
                    logbg_ing.style.display = 'none';
                }
                logging = false;
            });
        }
    }
}

//判断是不是安全中心
if(!/reg.99|aq.99|testreg.99/.test(location.hostname)){
    /**
     * JSONP调用
     */
    (function(w){
        function getjson(){}
        getjson.prototype.set=function(url,callback,callbackname){
            this.callfn=callbackname||'urlcallback';
            this.url=url+"&callBack="+this.callfn;
            try{
                eval(this.callfn+"=function(data){ "+
                    "callback(data); "+
                    'delete '+this.callfn+';}');
            }catch(e){return;}
            this.request();
            delete this.url;
        }
        getjson.prototype.request=function(){
            var script=document.createElement("script");
            script.src=this.url;
            var load=false;
            script.onload = script.onreadystatechange = function() {
                if(this.readyState === "loaded" || this.readyState === "complete"){
                    load=true;
                    script.onload = script.onreadystatechange=null;
                }
            };
            var head=document.getElementsByTagName("head")[0];
            head.insertBefore(script,head.firstChild);
        }
        w.getjson=getjson;
    })(window);
    function clearUserInfo(){
        //topGetE('total_num').innerHTML = "";
        // topGetE('total_num').style.display = 'none';
        // topGetE('menu_optb').style.display = 'none';
        // topGetE('reqs_a').style.display = 'none';
        // topGetE('pms_a').style.display = 'none';
        //  topGetE('fans_a').style.display = 'none';
        // topGetE('atme_a').style.display = 'none';
        // topGetE('reqs_num').innerHTML = "";

        // topGetE('pms_num').innerHTML = "";
        // topGetE('fans_num').innerHTML = "";
        // topGetE('atme_num').innerHTML = "";
        topGetE('_username').value = "";
        topGetE('_password').value = "";
    }
    /**
     * 从社区获取登录用户的信息
     * 该函数在topMenu()中最后一行有调用一次以在页面加载时就获取用户信息
     */
    function showUserInfo(log){
        clearUserInfo();
        var host = window.location.href.split('/')[2];
        var hostArr = host.split('.');
        var hostNum = hostArr[hostArr.length-2];
        var url = "https://t."+ hostNum +".com/?controller=js&action=userstatus";
        if(typeof log != 'undefined' && log){
            url += ("&log_site=" + encodeURI(location.host));
        }
        new getjson().set(url, function(data){
            if (data.status == 1) {
                //用户已登录
                topGetE('logbg_ing').style.display = 'none';
                topGetE('user_login').style.display = "none";
                if (btnnoshow == 1) {
                    topGetE('user_area').style.display = "block";
                }
                topGetE('user_nickname').innerHTML = data.nickname;

                topGetE('menu_jifen').innerHTML = data.point;
                var host1 = window.location.href.split('/')[2];
                var hostArr1 = host.split('.');
                var hostNum1 = hostArr[hostArr.length-2];
                topGetE('uh01').src = topGetE('uh02').src = "https://myreg."+ hostNum1 +".com/center/avatar.php?uid=" + data.uid + "&size=middle&type=real";
                initNewNotice(data.notice);
                hide_log_menu();
            }else{
                topGetE('user_login').style.display = "block";
                return false;
            }
        }, 'uinfoBack');

        if(!qdflag){
            var url = "https://myreg."+ hostNum +".com/port/SignAward.php?action=record&callBack=qdStatusCallback";

            new getjson().set(url, function(data){
                switch(data.flag){
                    case 0:
                        topGetE('continue_sign_count').innerHTML = "连续签到" + data.data.continue_signed_count + "天";
                        todaypoint = data.data.points;
                        break;
                    case 1:
                        topGetE('sign_btn').style.display = "none";
                        topGetE('sign_btn_disable').style.display = "block";
                        topGetE('sign_btn_disable').innerHTML = "本机已签到";
                        topGetE('continue_sign_count').innerHTML = "请勿重复签到";
                        break;
                    case 2:
                        topGetE('sign_btn').style.display = "none";
                        topGetE('sign_btn_disable').style.display = "block";
                        topGetE('continue_sign_count').innerHTML = "已连续签到" + data.data.continue_sign_count + "天";
                        break;
                    case 3:
                        topGetE('sign_btn').style.display = "none";
                        topGetE('sign_btn_disable').style.display = "block";
                        topGetE('sign_btn_disable').innerHTML = "签到已关闭";
                        break;
                    case -1:
                        ajaxLogOut();//登录超时
                        break;
                }
            }, 'qdStatusCallback');
        }
    }

    /**
     * 显示/刷新验证码
     */
    function showCheckCode(){
        var host = window.location.href.split('/')[2];
        var hostArr = host.split('.');
        var hostNum = hostArr[hostArr.length-2];
        var url = "https://aq."+ hostNum +".com/vcode.gif.ashx?pid=" + Math.random();
        topGetE('verify_img').src = url;
        topGetE('verifycode_box').style.display = 'block';
        return false;
    }

    /**
     * 初始化新通知信息
     */
    function initNewNotice(nums){
        var total_num = nums.reqs + nums.pm + nums.fans + nums.atme;
        if(total_num > 0){
            //topGetE('menu_optb').style.display = 'block';
            // topGetE('total_num').style.display = 'block';
            // topGetE('total_num').innerHTML = (total_num > 99) ? "99+" : total_num;
        }else{
            // topGetE('menu_optb').style.display = 'none';
        }
        if(nums.reqs > 0){
            // topGetE('reqs_num').innerHTML = '(' + nums.reqs + ')';
            // topGetE('reqs_a').style.display = 'block';
        }
        if(nums.pm > 0){
            // topGetE('pms_num').innerHTML = '(' + nums.pm + ')';
            // topGetE('pms_a').style.display = 'block';
        }
        if(nums.fans > 0){
            // topGetE('fans_num').innerHTML = '(' + nums.fans + ')';
            // topGetE('fans_a').style.display = 'block';
        }
        if(nums.atme > 0){
            // topGetE('atme_num').innerHTML = '(' + nums.atme + ')';
            // topGetE('atme_a').style.display = 'block';
        }
    }

    /**
     * 异步登出
     */

    function ajaxLogOut() {
        var host = window.location.href.split('/')[2];
        var hostArr = host.split('.');
        var hostNum = hostArr[hostArr.length-2];
        new getjson().set("https://t."+ hostNum +".com/?controller=js&action=logout", function(data){
            if(data.status == 1){
                var scriptList = data.srclist;
                var scriptOrder = 0;
                for(key in scriptList) {
                    gLogoutLoadScript('logout91Script_'+scriptOrder, scriptList[key]);
                    scriptOrder++;
                }
                topGetE('user_login').style.display = "block";
                topGetE('logbox').style.display = "none";
                topGetE('user_area').style.display = "none";
                topGetE('user_nickname').innerHTML = "";
                topGetE('sign_btn').style.display = "block";
                topGetE('sign_btn_disable').style.display = "none";

                clearUserInfo();
                document.location.reload();
            }
        });
        topGetE("imglogoff").src="https://myreg."+ hostNum +".com/Interface/UC/api/logout_user.php";
        return false;
    }

    function gLogoutLoadScript(id, src) {
        var head = document.getElementsByTagName("head").item(0);
        var oScript = document.getElementById(id);
        if (oScript) {
            head.removeChild(oScript);
        }
        oScript = document.createElement("script");
        oScript.setAttribute("id", id);
        oScript.setAttribute("src", src);
        oScript.setAttribute("type", "text/javascript");
        oScript.setAttribute("language", "javascript");
        head.appendChild(oScript);
    }
    var totalDate,todayDate;
    //获取日历,year年如：2012,month月如：10
    function getCalendar(year, month){

        month = month-1;

        //获取当前年月
        var myDate = new Date();
        todayDate = myDate.getDate();     //获取今日日期

        var lastDate = new Date(year, month+1, 0);  //获取当前月份的最后一天
        totalDate = lastDate.getDate();     //获取当前月份的总天数


        var isToday = 0;              //是否是今日，如果是为1，其他为0。
        var qdrlStr = '';             //返回的html串
        var firstweekday = 0;
        for(var i=0; i<totalDate; i++){
            var theMo = i%7;
            //获取每天是星期几
            var theDate = new Date(year, month, i+1);
            var weekday = theDate.getDay();

            if(todayDate == (i+1) && year == myDate.getFullYear() && month == myDate.getMonth()){
                isToday = 1;
            }else isToday = 0;

            //获取时间毫秒
            var datemill = theDate.getTime();

            var stap; //换行标记

            //补齐前面日期
            if(i == 0){
                firstweekday = weekday;
                qdrlStr = qdrlStr + '<tr class="date">';
                for(var j=0; j<weekday; j++){
                    var lastLen = weekday-1;//上个月的后lastLen天
                    var theDate = new Date(year, month, j-lastLen);

                    var day = theDate.getDate();
                    //获取时间毫秒
                    var firstDateMill = theDate.getTime();
                    qdrlStr = qdrlStr + '<td class="td_gray"><span>'+day+'</span></td>';
                }
                stap = 6-weekday;
            }

            qdrlStr = qdrlStr + '<td';
            if(isToday == 1){
                qdrlStr = qdrlStr + ' class="td_qdon"';
            }
            qdrlStr = qdrlStr + '><span>'+(i+1)+'</span><div class="doing clearfix" id="td_day'+(i+1)+'"></div></td>';
            if(theMo == stap){
                qdrlStr = qdrlStr + '</tr><tr class="date">';
            }

            //补齐后面日期

            if(i==totalDate-1){
                var firstLen = 42-firstweekday-totalDate;//下个月的前firstLen天
                for(var j=0; j<firstLen; j++){
                    if(weekday+j == 6){
                        qdrlStr = qdrlStr + '</tr>';

                        qdrlStr = qdrlStr + '<tr class="date">';
                    }
                    var theDate = new Date(year, month+1, j+1);
                    var day = theDate.getDate();
                    qdrlStr = qdrlStr + '<td class="td_gray"><span>'+day+'</span></td>';
                }
                qdrlStr = qdrlStr + '</tr>';
            }

        }

        return qdrlStr;
    }

    //签到日历
    function showqdrl(){
        if(!qdflag){//如果本机未重复签到

            var qdGames = new Array();
            qdGames[1] = '魔域';      //紫色
            qdGames[2] = '地下城守护者';  //褐色
            qdGames[3] = '征服';      //红色
            qdGames[4] = '机战';      //蓝色
            qdGames[5] = '英雄无敌';    //绿色
            qdGames[6] = '开心';      //粉红
            qdGames[7] = '梦幻迪士尼'; //黑色
            qdGames[8] = '投名状';   //黄色

            //图片中的点
            //1.#6E2BA1(紫色)
            //2.#E61A1A(红色)
            //3.#4DF15A(绿色)
            //4.#483434(黑色)
            //5.#9D7070(褐色)
            //6.#4ECBDC(蓝色)
            //7.#F7AFF7(粉红)
            //8.#B3B34D(黄色)

            var qdColor = new Array();
            qdColor[1] = 'purple';    //紫色
            qdColor[2] = 'brown';   //褐色
            qdColor[3] = 'red';     //红色
            qdColor[4] = 'blue';    //蓝色
            qdColor[5] = 'green';   //绿色
            qdColor[6] = 'pink';    //粉红
            qdColor[7] = 'dark';    //黑色
            qdColor[8] = 'yellow';    //黄色

            hide_log_menu();

            var total_point=0;  //总积分
            var t_point = 10; //今天获得积分
            var tom_point = 0;  //明天获得积分

            var myDate = new Date();

            var year = myDate.getFullYear();    //年份2012
            var month = myDate.getMonth()+1;    //月份。前面不要加0。

            var yearMonth = year+'年'+month+'月'; //显示的日历标题
            var qdrlStr = getCalendar(year, month);

            var qdpop = '<!--日历式签到-->'+
                '<div id="qdpop" class="qdpop qdpop_rl">'+
                ' <div id="qdpop_ing"><img src="https://img5.99.com/news/images/topmenu/0620/preload.gif"> 签到中，请稍候...</div>'+
                ' <div class="qdpop_hd"><a href="javascript:;" onclick="hideqd()" class="qdpop_shut">X</a><span>每日签到</span></div>'+
                ' <div class="qdpop_bd">'+
                '   <div id="qd_tip" class="qd_tip"><span id="t_point"> +'+t_point+'</span>'+
                '   <p id="tom_point">签到成功，明天继续签到将获得'+tom_point+'积分哦！ <a href="https://t.99.com/" title="签到详情">[签到详情]</a></p>'+
                '   <p>已连续签到<strong id="qd_continue">1</strong>天 | 总积分<strong id="total_point">'+total_point+'</strong> <a href="https://jf.99.com/?controller=member_score&action=exchange&source=rw" title="兑换">[兑换]</a></p></div>'+
                '   <!--日历-->'+
                '   <div class="qdpop_con rli_con"  style="">'+

                '     <div id="myrl">'+
                '       <form name="CLD">'+
                '       <table class="rl_box" style="" id="box" cellpadding="0" cellspacing="0" border="0">'+
                '         <tbody>'+
                '         <tr>'+
                '           <td align="center">'+
                '           <table class="biao" cellpadding="0">'+
                '             <tbody>'+
                '             <tr  class="rl_title" >'+
                '               <td colspan="7">'+yearMonth+'</td>'+
                '             </tr>'+
                '             <tr  class="week">'+
                '               <td width="70">日</td>'+
                '               <td width="70">一</td>'+
                '               <td width="70">二</td>'+
                '               <td width="70">三</td>'+
                '               <td width="70">四</td>'+
                '               <td width="70">五</td>'+
                '               <td width="70">六</td>'+
                '              </tr>'+
                qdrlStr +
                '                 </tbody>'+
                '         </table>'+

                '                  </td>'+
                '                </tr>'+
                '              </tbody>'+
                '            </table>'+
                '          </form>'+
                '        </div>'+
                '   </div>'+

                '   <!--/日历-->'+

                '      <!--活动介绍-->'+
                '      <div class="acts" id="acts_div"></div>'+
                '      <!--/活动介绍-->'+
                ' </div>'+
                ' <div class="qdpop_bottom"></div>'+
                '</div>'+
                '<!--日历式签到-->';

            TINY.box.show(qdpop,0,0,0,0,0,1);

            //获取用户信息
            var host = window.location.href.split('/')[2];
            var hostArr = host.split('.');
            var hostNum = hostArr[hostArr.length-2];
            var url = "https://t."+ hostNum +".com/?controller=js&action=userstatus&callBack=qdrlUinfoBack";
            new getjson().set(url, function(){}, 'qdrlUinfoBack');
            qdrlUinfoBack = function(data){
                if (data.status == 1) {
                    total_point = data.point;
                    setTimeout(function(){
                        if(topGetE('total_point')){
                            topGetE('total_point').innerHTML=total_point;
                        }
                    }, 1200);
                }
            }

            //获取用户的签到信息
            var url = "https://myreg."+ hostNum +".com/port/SignAward.php?action=record&callBack=qdrlCallback";
            new getjson().set(url, function(){}, 'qdrlCallback');
            qdrlCallback = function(data){
                switch(data.flag){
                    case 2:
                        switch (data.data.continue_sign_count){
                            case 1:  tom_point = tom_point + 20; break;
                            case 2:  tom_point = tom_point + 40; break;
                            case 3:  tom_point = tom_point + 70;break;
                            case 4:  tom_point = tom_point + 120; break;
                            default: tom_point = tom_point + 120;
                        }

                        t_point = data.data.point;
                        setTimeout(function(){
                            if(topGetE('tom_point')){
                                topGetE('tom_point').innerHTML = '签到成功，明天继续签到将获得'+tom_point+'积分哦！<a href="https://t.99.com/" title="签到详情"> [签到详情]</a>';
                            }
                            if(topGetE('t_point')){

                                topGetE('t_point').innerHTML = ' +' + t_point;
                            }
                            if(topGetE('qd_continue')){
                                topGetE('qd_continue').innerHTML = data.data.continue_sign_count;
                            }

                        },1200);
                        break;
                    case -1:
                        ajaxLogOut();//登录超时
                        break;
                    default:
                }
            }

            //获取活动列表
            if(month < 10) {
                var pmonth = '0'+month;
            }else pmonth = month;
            var dateString = year + '-' + pmonth;
            var url = "https://myreg."+ hostNum +".com/port/SignEvent.php?action=eventlist&month="+dateString+"&callBack=qdEventCallback";
            new getjson().set(url, function(){}, 'qdEventCallback');
            qdEventCallback = function(data){
                var eventArr = new Array(); //开一个数组存放日历每天的活动

                var flag = data.flag;   //标记、只有为1的时候才是返回正确。
                if(flag == 1){
                    var data = data.data; //返回的数据
                    if(data != null){
                        var len = data.length;  //返回的数据条数
                        var actsStr = '<ul class="clearfix">';  //底部活动
                        for(var i=0; i<len; i++){
                            var title =data[i].title;
                            var eventurl = data[i].eventurl;
                            var games = data[i].games;
                            var eventstartdate = data[i].eventstartdate;
                            var eventenddate = data[i].eventenddate;
                            //计算活动的天数
                            var intervalTime = (eventenddate-eventstartdate)*1000;  //两个日期相差的毫秒数 一天86400000毫秒
                            var intervalDays = parseInt(Math.abs(intervalTime) / 86400000)+1;   //把相差的毫秒数转换为天数。+1为了在同一天的时候返回1。

                            var eventDate = new Date();
                            eventDate.setTime(eventstartdate+'000');
                            var eventDateTime = eventDate.getTime();
                            var startday = eventDate.getDate();
                            //从活动开始遍历
                            var leftday = totalDate-startday;
                            for(var s=startday,l=(leftday>intervalDays)?intervalDays:leftday+1; s<startday+l; s++){
                                var obj = topGetE('td_day'+s);
                                obj.innerHTML = obj.innerHTML + '<em class="do_'+games+'"></em>';
                            }

                            actsStr = actsStr + '<li><span class="'+qdColor[games]+'">'+qdGames[games]+'</span><a href="'+eventurl+'" target="_blank">'+title+'</a></li>';
                        }
                        actsStr = actsStr + '</ul>';
                        setTimeout(function(){
                            if(topGetE('acts_div')){
                                topGetE('acts_div').innerHTML=actsStr;
                            }
                        }, 1200);
                    }
                }else{
                    //活动数据调用失败处理
                }
            }

            //获取用户的签到列表
            if(month < 10) {
                var pmonth = '0'+month;
            }else pmonth = month;
            var dateString = year + '' + pmonth + '01';
            var url = "https://myreg."+ hostNum +".com/port/SignAward.php?action=signDate&month="+dateString;
            new getjson().set(url, function(){}, 'qdDateCallback');
            qdDateCallback = function(data){
                var flag = data.flag;   //标记、只有为1的时候才是返回正确。
                if(flag == 1){
                    var data = data.data; //返回的数据
                    if(data != null){
                        var len = data.length;
                        var tempDate = new Date();
                        setTimeout(function(){
                            for(var i=0; i<len; i++){
                                var signTime = data[i].sign_date + '000';
                                tempDate.setTime(signTime);
                                var tempday = tempDate.getDate();
                                if(topGetE('td_day'+tempday)){
                                    topGetE('td_day'+tempday).parentNode.className = 'td_qdon';
                                }
                            }
                        }, 1200);

                    }
                }else{
                    //数据调用失败处理
                }
            }
        }
    }

    function showqd(){ //去签到
        if(topGetE('sign_btn_disable').style.display == "none"){
            var host = window.location.href.split('/')[2];
            var hostArr = host.split('.');
            var hostNum = hostArr[hostArr.length-2];
            hide_log_menu();
            var qdtips = ['生命不在于活得长与短，而在于顿悟的早与晚。，',
                '讨厌的星期一，我还没睡够呢！',
                '今天是星期二，',
                '今天是星期三，',
                '今天是星期四，',
                '终于星期五了哦，加油！',
                '解放了，周末怎么玩呢？'
            ];
            var topDate = (new Date()).getDay();
            var qdpop = '<div id="qdpop" class="qdpop">'+
                ' <div id="qdpop_ing"><img src="https://img5.99.com/news/images/topmenu/0620/preload.gif" /> 签到中，请稍候...</div>'+
                ' <div class="qdpop_hd">'+
                '   <a class="qdpop_shut" onclick="hideqd()" href="javascript:;">X</a>'+
                '   <span>每日签到</span>'+
                ' </div>'+
                ' <div class="qdpop_bd">';
            qdpop += '<div id="qd_tip">'+ qdtips[topDate] +'请选择今天的表情并写下你的心情！</div>'+
                '   <ul class="clearfix pop_qd">'+
                '     <li><a href="#" id="qd_face_tab" onclick="changeTab(\'qd_face_tab\');" title="心情签到" class="on">心情签到</a></li>'+
                '     <li><a href="#" id="qd_activity_tab" onclick="changeTab(\'qd_activity_tab\');" title="活动话题" style="display:none;">活动话题</a></li>'+
                '   </ul>'+
                '   <div class="qdpop_con">'+
                '   <ul class="qd_face" id="qd_face">'+
                '     <li id="topface2" onclick="setface(2)" class="on"><img src="https://img5.99.com/pz91/images/smile/2.gif" /> <span>开心</span></li>'+
                '     <li id="topface13" onclick="setface(13)"><img src="https://img5.99.com/pz91/images/smile/13.gif" /> <span>难过</span></li>'+
                '     <li id="topface11" onclick="setface(11)"><img src="https://img5.99.com/pz91/images/smile/11.gif" /> <span>加油</span></li>'+
                '     <li id="topface23" onclick="setface(23)"><img src="https://img5.99.com/pz91/images/smile/23.gif" /> <span>无语</span></li>'+
                '     <li id="topface19" onclick="setface(19)"><img src="https://img5.99.com/pz91/images/smile/19.gif" /> <span>生气</span></li>'+
                '     <li id="topface7" onclick="setface(7)"><img src="https://img5.99.com/pz91/images/smile/7.gif" /> <span>发呆</span></li>'+
                '     <li id="topface12" onclick="setface(12)"><img src="https://img5.99.com/pz91/images/smile/12.gif" /> <span>得意</span></li>'+
                '   </ul>'+

                '   <ul id="qd_activity" class="qd_face qd_ht" style="display:none">'+
                '     <li class="on" onclick="setface(2001)" id="topface2001"><img src="https://img5.99.com/t91/v2/images/smile/2001.gif"> <span>魔域</span></li>'+
                '     <li onclick="setface(2002)" id="topface2002"><img src="https://img5.99.com/t91/v2/images/smile/2002.gif"> <span>征服</span></li>'+
                '     <li onclick="setface(2003)" id="topface2003"><img src="https://img5.99.com/t91/v2/images/smile/2003.gif"> <span>DK</span></li>'+
                '     <li onclick="setface(2004)" id="topface2004"><img src="https://img5.99.com/t91/v2/images/smile/2004.gif"> <span>英雄无敌</span></li>'+

                '     <li onclick="setface(2005)" id="topface2005"><img src="https://img5.99.com/t91/v2/images/smile/2005.gif"> <span>机战</span></li>'+
                '     <li onclick="setface(2006)" id="topface2006"><img src="https://img5.99.com/t91/v2/images/smile/2006.gif"> <span>开心</span></li>'+
                '     <li onclick="setface(2007)" id="topface2007"><img src="https://img5.99.com/t91/v2/images/smile/2007.gif"> <span>梦迪</span></li>'+
                '     <li onclick="setface(2008)" id="topface2008"><img src="https://img5.99.com/t91/v2/images/smile/2008.gif"> <span>投名状</span></li>'+
                '   </ul>'+

                '   <div class="qb_mood">'+
                '     <textarea id="qd_mood" class="qd_myword"  >写下你今天的心情吧！</textarea> '+
                '   </div>'+
                '   <div class="qd_btn">'+
                '     <span class="ltc1">验证码：</span>'+
                '     <span class="ltc2"><input id="qd_code" type="text" maxlength="4" name="code" /><img id="qd_codeimg" src="https://myreg.'+ hostNum +'.com/?controller=public&action=verifycode" onclick="showQdCode();" title="点击更换验证码" /></span>'+
                '     <a class="fu_qd" href="javascript:;" onclick="return qdPost()">立即签到</a> <!--<span id="today_sign_point">今天签到可以领取' + todaypoint + '积分</span>-->'+
                '   </div>'+
                '   <div id="qb_mood_tip"></div>'+
                '   <div id="qd_activity_news" class="ht_con">'+
                '     <a href="'+sqhdHref+'" title="'+sqhdTitle+'" target="_blank">'+sqhdTitle+'</a>'+
                '   </div>'+
                '   </div>'+
                ' </div>'+
                ' <div class="qdpop_bottom"></div>'+
                '</div>';
            TINY.box.show(qdpop,0,0,0,0,0,1);
        }
    }

    //改变tab标签
    function changeTab(li_id){
        if(li_id == 'qd_face_tab'){
            topGetE('qd_face_tab').className = "on";
            topGetE('qd_activity_tab').className = "";
            topGetE('qd_activity_news').style.display="";
            topGetE('qd_face').style.display="";
            topGetE('qd_activity').style.display="none";
        }else{
            topGetE('qd_face_tab').className = "";
            topGetE('qd_activity_tab').className = "on";
            topGetE('qd_activity_news').style.display="";
            topGetE('qd_face').style.display="none";
            topGetE('qd_activity').style.display="";
        }
    }

    function hideqd(){ //显示签到写心情
        topGetE('qdpop_ing').style.display="none";
        TINY.box.hide();
    }

    function showQdCode(){
        var host = window.location.href.split('/')[2];
        var hostArr = host.split('.');
        var hostNum = hostArr[hostArr.length-2];
        var url = "https://myreg."+ hostNum +".com/?controller=public&action=verifycode&randomno=" + Math.random();
        topGetE('qd_codeimg').src = url;
        topGetE('qd_code').value = '';
        topGetE('qd_code').focus();
        return false;
    }

    function qdPost(){ //发表签到
        var qb_mood_tip = topGetE('qb_mood_tip');
        qb_mood_tip.style.display = "none";
        var face =topGetE('topface').value;
        var mood = topGetE('qd_mood').value;
        var code = topGetE('qd_code').value;
        var host = window.location.href.split('/')[2];
        var hostArr = host.split('.');
        var hostNum = hostArr[hostArr.length-2];
        var qdUrl = "https://myreg."+ hostNum +".com/port/SignAward.php?action=award";
        var s = '<div id="qdwin">';
        if (code == '') {
            qb_mood_tip.innerHTML = "请填写验证码！";
            qb_mood_tip.style.display = "inline-block";
        }else{
            topGetE('qdpop_ing').style.display="block";
            new getjson().set(encodeURI(qdUrl + "&mood=" + face + mood + "&code=" + code), function(data){
                if(data.flag ==0 ){
                    lucky_point = 0;
                    con_point = 0;
                    s += '签到成功，你获得了' + data.data.normal_point + '积分';
                    if(data.data.lucky_name){
                        s += '，' + data.data.lucky_name;
                        if(data.data.lucky_point){lucky_point = data.data.lucky_point; s += lucky_point + '积分';}
                        if(data.data.lucky_frozenbz){s += data.data.lucky_frozenbz + '个冰包子';}
                    }
                    if(data.data.weekly_point||data.data.weekly_frozenbz||data.data.monthly_point||data.data.monthly_frozenbz){
                        s += '，满勤奖励';
                        con_point = (data.data.weekly_point)?data.data.weekly_point:0 + (data.data.monthly_point)?data.data.monthly_point:0;
                        frozenbzs = (data.data.weekly_frozenbz)?data.data.weekly_frozenbz:0 + (data.data.monthly_frozenbz)?data.data.monthly_frozenbz:0;
                        if (con_point!=0){s += con_point + '积分';}
                        if (frozenbzs!=0){s += frozenbzs + '个冰包子';}
                    }
                    s += '。明天继续签到将获得';
                    switch (data.data.continue_sign_count){
                        case 1:  s+='20'; break;
                        case 2:  s+='40'; break;
                        case 3:  s+='70'; break;
                        case 4:  s+='120'; break;
                        default: s+='120';
                    }
                    s += '积分哦！<a href="https://t.99.com" target="_blank">>>查看我的签到</a>';
                    topGetE('sign_btn').style.display = "none";
                    topGetE('sign_btn_disable').style.display = "block";
                    topGetE('continue_sign_count').innerHTML = "已连续签到" + data.data.continue_sign_count + "天";
                    topGetE('menu_jifen').innerHTML = parseInt(topGetE('menu_jifen').innerHTML) + parseInt(data.data.normal_point) + parseInt(lucky_point) + parseInt(con_point);
                    //签到成功直接跳转到日历
                    hideqd();
                    showqdrl();
                }else if(data.flag ==1 ){
                    topGetE('sign_btn').style.display = "none";
                    topGetE('sign_btn_disable').style.display = "block";
                    topGetE('sign_btn_disable').innerHTML = "本机已签到";
                    topGetE('continue_sign_count').innerHTML = "请勿重复签到"
                    s +="本机已经签到过了，请勿重复签到哦！";
                    qdflag = true;
                    //签到成功直接跳转到日历
                    hideqd();
                    showqdrl();
                }else if(data.flag ==2 ){
                    topGetE('sign_btn').style.display = "none";
                    topGetE('sign_btn_disable').style.display = "block";
                    topGetE('continue_sign_count').innerHTML = "已连续签到" + data.data.continue_sign_count + "天";
                    s += "您今天已经签到过了哦！";
                    //签到成功直接跳转到日历
                    hideqd();
                    showqdrl();
                }else if(data.flag ==3){
                    topGetE('sign_btn').style.display = "none";
                    topGetE('sign_btn_disable').style.display = "block";
                    topGetE('continue_sign_count').innerHTML = "签到已关闭";
                    s += "抱歉，签到活动已关闭，感谢您的支持！";
                    //签到成功直接跳转到日历
                    hideqd();
                    showqdrl();
                }else if(data.flag ==5){
                    qb_mood_tip.innerHTML = "验证码错误，请重输！";
                    qb_mood_tip.style.display = "inline-block";
                    showQdCode();
                }else{
                    qb_mood_tip.innerHTML = "抱歉，签到出错，请重新登录后重试！";
                    qb_mood_tip.style.display = "inline-block";
                }

                s +="</div>";
                topGetE('qdpop_ing').style.display="none";
                //    TINY.box.show(s,0,0,1,5,1);

            });
            return false;
        }
    }
    var face_msg = new Array();
    face_msg['2']= '保持好心情，就会有好运哦！';
    face_msg['13']= '我不难过~只是为什么眼泪会流~我也不懂~';
    face_msg['11']= '大家加油加油！！';
    face_msg['23']= '￥%#……谁也不要理我！';
    face_msg['19']= '哪凉快哪呆着去！';
    face_msg['7']= '偷得浮生半日闲~唔~~（伸懒腰）';
    face_msg['12']= '我得意的笑~我得意的笑~！';
    face_msg['2001']= '#魔域#';
    face_msg['2002']= '#征服#';
    face_msg['2003']= '#DK#';
    face_msg['2004']= '#英雄无敌#';
    face_msg['2005']= '#机站#';
    face_msg['2006']= '#开心#';
    face_msg['2007']= '#梦迪#';

    face_msg['2008']= '#投名状#';
    function setface(f){ //设置心情
        topGetE('topface').value = "[f="+f+"]";
        var li = topGetE('qd_face').getElementsByTagName("li");
        for(var i = 0; i < li.length; i++){
            li[i].className = "";
        }
        topGetE('topface'+f).className = "on";
    }
    var rTasks = {ts:[
            {"i":899,"n":"下载魔域盒子","j":100,"e":100,"d":"下载安装【魔域盒子】（安卓版、IOS版均可），游戏好友随叫随到！游戏更新活动提醒，资讯爆料触手可及！"},
            {"i":870,"n":"每日一答","j":20,"e":20,"d":"每天一个回答，让你迅速成为问吧的知识达人！开始挑战吧！"},
            {"i":225,"n":"扫零初战","j":30,"e":30,"d":"问吧还有许多0回复问题无人问津，任务门决定动员全民开展扫零计划，快用知识武装自己，参加这次的扫零初战吧！"},
            {"i":244,"n":"智者的提示","j":50,"e":30,"d":"问吧的智者总对新人关怀备至，在他们的摸索旅程上给予些小提示，并常常鼓励他们进行新的尝试，于是挑战悬赏分就成为新人必做的任务之一。"},
            {"i":861,"n":"小游戏打分","j":10,"e":10,"d":"你有喜欢的小游戏，如果觉得很有趣，就为它们打分吧"},
            {"i":428,"n":"池塘边","j":30,"e":36,"d":"在池塘边钓鱼，也许还有惊奇的发现，或许还能在池塘边钓上圣诞老人的靴子。"},
            {"i":516,"n":"孤岛求生","j":30,"e":36,"d":"假设你落入一个无人的孤岛，四面皆是茫茫大海，而身边只有一副渔具。饥肠辘辘的你，即使没有钓鱼经验，也只好撞撞运气，猜猜下一秒会钓出什么呢？"},
            {"i":872,"n":"截图达人","j":20,"e":20,"d":"秀秀你的武器装备、幻兽宝宝、或者游戏中那一抹风景，快上次你的游戏截图吧！"},
            {"i":871,"n":"论坛神人","j":20,"e":20,"d":"99论坛的每个帖子是否都留下你的足迹，从这一刻开始，成为论坛的回帖神人！"}
        ]};

    function genTasks(ids){
        genstr = "";
        for(i=0; i<ids.length; i++){
            genstr += "<div";
            genstr += (i==ids.length-1)?" class=\"last\">":">";
            genstr += "<a href=\"https://rw.99.com/?controller=rwtask&action=detail&id=" + rTasks.ts[ids[i]].i + "\" title=\"" + rTasks.ts[ids[i]].d +"\" target=_blank>" + rTasks.ts[ids[i]].n +"</a> <span class=\"task_li_exp\"><em></em>" + rTasks.ts[ids[i]].e +"</span> <span class=\"task_li_jf\"><em></em>" + rTasks.ts[ids[i]].j +"</span> </div>";
        }
        return genstr;
    }

    function showTaskList(){
        var topDomainName =  document.domain || "www.99.com";
        var topDomainFlag = topDomainName.substring(0,topDomainName.indexOf(".99"));
        var tli = topGetE("task_li");
        switch(topDomainFlag){
            case "bbs": tli.innerHTML = genTasks([8,6,7]); break;
            default: tli.innerHTML = genTasks([8,6,7]);
        }
    }
    var TINY={};

    function T$(i){return document.getElementById(i);}
    TINY.box=function(){
        var p,m,b,fn,ic,iw,ih,ia,f=0;
        return{
            show:function(c,w,h,a,t,newt){

                if(!f){
                    p=document.createElement('div'); p.id='tinybox';
                    b=document.createElement('div'); b.id='tinycontent';
                    document.body.appendChild(p); p.appendChild(b);
                    window.onresize=TINY.box.resize; f=1;
                }
                if(!a){
                    p.style.width=w?w+'px':'auto'; p.style.height=h?h+'px':'auto';
                    p.style.backgroundImage='none'; b.innerHTML=c;
                }else if(!newt){
                    b.style.display='none'; p.style.width=p.style.height='100px';
                }
                ic=c; iw=w; ih=h; ia=a;
                if(newt){
                    this.fill(ic,iw,ih,ia);
                }else{
                    this.alpha(p,1,95,3);
                }
                if(t){setTimeout(function(){TINY.box.hide();},1000*t);}
            },
            fill:function(c,w,h,a){
                this.psh(c,w,h,a);
            },
            psh:function(c,w,h,a){
                if(a){
                    if(!w||!h){
                        var x=p.style.width, y=p.style.height; b.innerHTML=c;
                        p.style.width=w?w+'px':''; p.style.height=h?h+'px':'';
                        b.style.display='';
                        w=parseInt(b.offsetWidth); h=parseInt(b.offsetHeight);
                        b.style.display='none'; p.style.width=x; p.style.height=y;
                    }else{
                        b.innerHTML=c;
                    }
                    this.size(p,w,h);
                }else{
                    p.style.backgroundImage='none';
                }
            },
            hide:function(){
                TINY.box.alphahide(p,0);
            },
            resize:function(){
                TINY.box.pos();
            },
            pos:function(){
                var t=(TINY.page.height()/2)-(p.offsetHeight/2); t=t<10?10:t;
                p.style.top=(t+TINY.page.top())+'px';
                p.style.left=(TINY.page.width()/2)-(p.offsetWidth/2)+'px';
            },
            alpha:function(e,d,a){
                clearInterval(e.ai);
                if(d==1){
                    e.style.opacity=0; e.style.filter='alpha(opacity=0)';
                    e.style.display='block'; this.pos();
                }
                e.ai=setInterval(function(){TINY.box.ta(e,a,d);},20);
            },
            ta:function(e,a,d){
                var o=Math.round(e.style.opacity*100);
                if(o==a){
                    clearInterval(e.ai);
                    if(d==-1){
                        TINY.box.alpha(p,-1,0,2);
                        b.innerHTML=p.style.backgroundImage='';

                    }else{
                        TINY.box.fill(ic,iw,ih,ia);
                    }
                }else{
                    var n=Math.ceil(((a+o)*.5)); n=n==1?0:n;
                    e.style.opacity=n/100; e.style.filter='alpha(opacity='+n+')';
                }
            },
            alphahide:function(e,a){
                clearInterval(e.ai);
                e.ai=setInterval(function(){TINY.box.tb(e,a);},20);
            },
            tb:function(e,a){
                var o=Math.round(e.style.opacity*100);
                if(o==a){
                    clearInterval(e.ai);
                    e.style.opacity=0; e.style.filter='alpha(opacity=0)';
                    b.innerHTML=p.style.backgroundImage='';
                }else{
                    var n=Math.ceil(((a+o)*.5)); n=n==1?0:n;
                    e.style.opacity=n/100; e.style.filter='alpha(opacity='+n+')';
                }
            },

            size:function(e,w,h){
                e=typeof e=='object'?e:T$(e); clearInterval(e.si);
                var ow=e.offsetWidth, oh=e.offsetHeight,
                    wo=ow-parseInt(e.style.width), ho=oh-parseInt(e.style.height);
                var wd=ow-wo>w?0:1, hd=(oh-ho>h)?0:1;
                e.si=setInterval(function(){TINY.box.ts(e,w,wo,wd,h,ho,hd);},20);
            },
            ts:function(e,w,wo,wd,h,ho,hd){
                var ow=e.offsetWidth-wo, oh=e.offsetHeight-ho;

                if(ow==w&&oh==h){
                    clearInterval(e.si); p.style.backgroundImage='none'; b.style.display='block';
                }else{
                    if(ow!=w){var n=ow+((w-ow)*.5); e.style.width=wd?Math.ceil(n)+'px':Math.floor(n)+'px';}
                    if(oh!=h){var n=oh+((h-oh)*.5); e.style.height=hd?Math.ceil(n)+'px':Math.floor(n)+'px';}
                    this.pos();
                }
            }
        }
    }();
    TINY.page=function(){
        return{
            top:function(){return document.documentElement.scrollTop||document.body.scrollTop;},
            width:function(){return self.innerWidth||document.documentElement.clientWidth||document.body.clientWidth;},
            height:function(){return self.innerHeight||document.documentElement.clientHeight||document.body.clientHeight;},

            total:function(d){
                var b=document.body, e=document.documentElement;
                return d?Math.max(Math.max(b.scrollHeight,e.scrollHeight),Math.max(b.clientHeight,e.clientHeight)):
                    Math.max(Math.max(b.scrollWidth,e.scrollWidth),Math.max(b.clientWidth,e.clientWidth));
            }
        }
    }();
}
