#ifndef WIN_HELPER_CPP
#define WIN_HELPER_CPP

#include <Windows.h>

namespace win_helper {
    inline BOOL CALLBACK EnumWindowsProc(HWND hwnd, LPARAM lParam) {
        DWORD processId;
        GetWindowThreadProcessId(hwnd, &processId);

        // Kiểm tra nếu cửa sổ thuộc về quá trình hiện tại
        if (processId == GetCurrentProcessId()) {
            // Lưu HWND vào lParam
            *(HWND*)lParam = hwnd;
            return FALSE;  // Dừng việc liệt kê cửa sổ
        }
        return TRUE;  // Tiếp tục liệt kê cửa sổ
    }

    // Hàm để lấy HWND từ quá trình hiện tại
    inline HWND GetCurrentProcessWindow() {
        HWND hwnd = nullptr;
        EnumWindows(EnumWindowsProc, (LPARAM)&hwnd);
        return hwnd;
    }
}

#endif // !WIN_HELPER_CPP
