#include "graphic_hook.h"
namespace graphic_hook {
	void Init() {
        DetourTransactionBegin();
        DetourUpdateThread(GetCurrentThread());
        oShowString = (ShowString_t)DetourFindFunction("graphic.dll", "?ShowString@CMyBitmap@@SAXHHKPBD0HHW4RENDER_TEXT_STYLE@@KAAUCMyPos@@@Z");

        DWORD dwEndScene = FindPattern((char*)"d3d8.dll",
            (char*)"\x6A\x18\xB8\x00\x00\x00\x00\xE8\x00\x00\x00\x00\x8B\x7D\x08\x8B\xDF\x8D\x47\x04\xF7\xDB\x1B\xDB\x23\xD8\x89\x5D\xE0\x33\xF6\x89\x75\xE4\x39\x73\x18\x75\x73",
            (char*)"xxx????x????xxxxxxxxxxxxxxxxxxxxxxxxxxx");

        DetourAttach(&(PVOID&)oShowString, Hooked_ShowString);

        DetourTransactionCommit();
	}

    void Destroy() {
        DetourTransactionBegin();
        DetourUpdateThread(GetCurrentThread());

        DetourDetach(&(PVOID&)oShowString, Hooked_ShowString);

        DetourTransactionCommit();
    }

    std::wstring ConvertToWString(const char8_t* utf8Str) {
        int utf8StrLength = -1; // -1 để tính toán độ dài chuỗi
        int wideCharLength = MultiByteToWideChar(CP_UTF8, 0, (LPCCH)utf8Str, utf8StrLength, NULL, 0);

        std::wstring wideStr(wideCharLength, L'\0');
        MultiByteToWideChar(CP_UTF8, 0, (LPCCH)utf8Str, utf8StrLength, wideStr.data(), wideCharLength);

        return wideStr;
    }

    // Hàm vẽ văn bản UTF-8
    void DrawUtf8Text(HDC hdc, int x, int y, const char8_t* utf8Text) {
        // Chuyển đổi UTF-8 (char8_t*) sang UTF-16 (wchar_t*)
        std::wstring utf16Text = ConvertToWString(utf8Text);

        // Thiết lập font và vẽ văn bản
        HFONT hFont = CreateFontW(20, 0, 0, 0, FW_NORMAL, FALSE, FALSE, FALSE,
            DEFAULT_CHARSET, OUT_OUTLINE_PRECIS,
            CLIP_DEFAULT_PRECIS, CLEARTYPE_QUALITY,
            VARIABLE_PITCH, L"Arial");
        SelectObject(hdc, hFont);

        TextOutW(hdc, x, y, utf16Text.c_str(), utf16Text.length());

        // Dọn dẹp
        DeleteObject(hFont);
    }

    void __cdecl Hooked_ShowString(int a, int b, unsigned long c, const char* text, const char* e, int f, int g, RENDER_TEXT_STYLE h, unsigned long i, CMyPos& pos)
    {
        PAINTSTRUCT ps;
        HWND hwnd = GetForegroundWindow();
        HDC hdc = BeginPaint(hwnd, &ps);

        // Văn bản tiếng Việt có dấu
        auto newText = u8"Chào thế giới!";
        // Gọi hàm để vẽ văn bản
        DrawUtf8Text(hdc, a, b, newText);

        EndPaint(hwnd, &ps);
        return;
    }

    DWORD_PTR* FindDevice(DWORD Base, DWORD Len)
    {
        unsigned long i = 0, n = 0;

        for (i = 0; i < Len; i++)
        {
            if (*(BYTE*)(Base + i + 0x00) == 0xC7)n++;
            if (*(BYTE*)(Base + i + 0x01) == 0x06)n++;
            if (*(BYTE*)(Base + i + 0x06) == 0x89)n++;
            if (*(BYTE*)(Base + i + 0x07) == 0x86)n++;
            if (*(BYTE*)(Base + i + 0x0C) == 0x89)n++;
            if (*(BYTE*)(Base + i + 0x0D) == 0x86)n++;
            if (n == 6) return (DWORD_PTR*)(Base + i + 2); n = 0;
        }
        return(0);
    }


    HRESULT __stdcall Hooked_Present(LPDIRECT3DDEVICE8 pDevice, CONST RECT* pSourceRect, CONST RECT* pDestRect, HWND hDestWindowOverride, CONST RGNDATA* pDirtyRegion)
    {
        return  oPresent(pDevice, pSourceRect, pDestRect, hDestWindowOverride, pDirtyRegion);
    }
}