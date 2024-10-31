#pragma once
class IEmoneyChk {
public:
    // Hàm khởi tạo
    IEmoneyChk() {}

    // Hàm hủy
    virtual ~IEmoneyChk() {}

    // Hàm tạo mới
    static IEmoneyChk* CreateNew();

    // Hàm tính toán tổng kiểm tra cho thẻ
    virtual unsigned long CalcCardChkSum(int /*cardType*/, int /*cardValue*/, int /*cardSerial*/);

    // Hàm tính toán tổng kiểm tra cho vật phẩm
    virtual unsigned long CalcItemChkSum(int /*itemId*/, int /*itemValue*/);

    // Hàm tính toán tổng kiểm tra mở rộng cho vật phẩm
    virtual unsigned long CalcItemExChkSum(int /*itemId*/, int /*itemValue*/,
        unsigned char /*param1*/, unsigned char /*param2*/,
        unsigned char /*param3*/);

    // Hàm tính toán tổng kiểm tra cho RMB
    virtual unsigned long CalcRMBChkSum(int /*rmbType*/, int /*rmbValue*/);

    // Hàm tính toán tổng kiểm tra cho robot
    virtual unsigned long CalcRobotChkSum(unsigned char /*robotType*/, unsigned char /*robotLevel*/,int /*robotId*/);

    // Hàm tính toán tổng kiểm tra cho người dùng
    virtual unsigned long CalcUserChkSum(int /*userId*/, unsigned __int64 /*userAccount*/,int /*userLevel*/);

    // Hàm tính toán tổng kiểm tra mở rộng cho người dùng
    virtual unsigned long CalcUserXChkSum(unsigned __int64 /*userAccount*/, int /*userId*/, int /*userLevel*/, int /*userRank*/);

    // Hàm giải phóng tài nguyên
    virtual unsigned long Release();
};