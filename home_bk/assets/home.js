$(document).ready(function () {
	$(".lunbo").slide({titCell: ".page ul", mainCell: ".lunbo-cont ul", autoPage: true, autoPlay: true});
})
window.onLoadTab = function () {
	tfchg("tab_ti", "tab_cont");
	setTdWidth("tab_cont");
}
