$(document).ready(()=>{
    var lunboSwiper = new Swiper('#lunboSwiper',{
        pagination: '.pagination',
        autoplay: 3000,
        loop: true,
        paginationClickable: true
    })

//轮播资讯
    var mySwiper2 = new Swiper("#newSwiper", {
        onSlideChangeEnd: function (swiper) {
            $("#swBtn a").eq(mySwiper2.activeIndex).addClass("on").siblings().removeClass("on");
        }
    });
    $("#swBtn").on("click",'a', function (e) {
        e.preventDefault();
        $(this).addClass("on").siblings().removeClass("on");
        var i = $(this).index();
        mySwiper2.swipeTo(i, 1000, false);
    });

//职业轮播
    var $zyContentShow = $("#zyContent .zy-k"),
        $zyLBtn = $("#zyLBtn"),
        $zyRBtn = $("#zyRBtn");
    var mySwiper3 = new Swiper(".m2-content .swiper-container", {
        loop: true,
        slidesPerView: 'auto',
        loopedSlides: 3,
        initialSlide: 0,
        keyboardControl: false,
        centeredSlides: true,
        noSwiping: true,
        onSlideChangeEnd: function (swiper) {
            var zyIndex = mySwiper3.activeLoopIndex;
            $zyContentShow.eq(zyIndex).addClass("show").siblings().removeClass("show");
        }
    });

    $zyLBtn.click(function () {
        mySwiper3.swipePrev();
    });
    $zyRBtn.click(function () {
        mySwiper3.swipeNext();
    });

})
