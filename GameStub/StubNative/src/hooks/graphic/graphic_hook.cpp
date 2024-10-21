#include "graphic_hook.h"
#define G_D3DDEVICE_ADDRESS 0x00B4FE20

namespace graphic_hook {
    typedef HRESULT(APIENTRY* EndScene_t)(LPDIRECT3DDEVICE8 pDevice);



	void Init() {
        DetourTransactionBegin();
        DetourUpdateThread(GetCurrentThread());
        /*oShowString = (ShowString_t)DetourFindFunction("graphic.dll", "?ShowString@CMyBitmap@@SAXHHKPBD0HHW4RENDER_TEXT_STYLE@@KAAUCMyPos@@@Z");

        DWORD gDeviceOffset = 0xac3e4;
        HMODULE hBaseAddress = GetModuleHandle("C3_CORE_DLL.dll");
        if(hBaseAddress)
        {
            uintptr_t g_D3DDeviceAddress = (uintptr_t)GetProcAddress(hBaseAddress, "g_D3DDevice");

            if (g_D3DDeviceAddress == NULL)
            {
                g_D3DDeviceAddress = G_D3DDEVICE_ADDRESS;
            }

            auto g_D3DDevice = (IDirect3DDevice8*)g_D3DDeviceAddress;
            if (g_D3DDevice)
            {

            }
        }
        DetourAttach(&(PVOID&)oShowString, Hooked_ShowString);*/



        DetourTransactionCommit();
	}

    void Destroy() {
        DetourTransactionBegin();
        DetourUpdateThread(GetCurrentThread());

        //DetourDetach(&(PVOID&)oShowString, Hooked_ShowString);

        DetourTransactionCommit();
    }


    void __cdecl Hooked_ShowString(int startX, int startY, unsigned long c, const char* text, const char* e, int f, int g, RENDER_TEXT_STYLE h, unsigned long i, CMyPos* pos)
    {
        oShowString(startX, startY, c, text, e, f, g, h, i, pos);
        return;
    }

    DWORD_PTR* FindDevice(DWORD Base, DWORD Len)
    {
        unsigned long i = 0, n = 0;

        for (i = 0; i < Len; i++)
        {
            if (*(BYTE*)(Base + i + 0x00) == 0xC7)n++;
            if (*(BYTE*)(Base + i + 0x01) == 0x06)n++;
            if (*(BYTE*)(Base + i + 0x06) == 0x88)n++;
            if (*(BYTE*)(Base + i + 0x07) == 0x86)n++;
            if (*(BYTE*)(Base + i + 0x0C) == 0x88)n++;
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