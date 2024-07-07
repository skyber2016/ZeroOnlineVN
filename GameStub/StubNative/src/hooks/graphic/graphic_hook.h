#ifndef GRAPHIC_HOOK_H
#define GRAPHIC_HOOK_H

#include "../../helpers/sigscan.hpp"
#include "../../variables/type_definations.hpp"
#include <detours.h>
#include <iostream>

namespace graphic_hook
{
	void Init();
	void Destroy();
	inline ShowString_t oShowString = nullptr;
	inline ShowTString_t oShowTString = nullptr;
	inline ShowStringEx_t oShowStringEx = nullptr;
	inline Present_t oPresent = nullptr;
	inline EndScene_t originalEndScene = nullptr;
	DWORD_PTR* FindDevice(DWORD Base, DWORD Len);
	void __cdecl Hooked_ShowString(int a, int b, unsigned long c, const char* text, const char* e, int f, int g, RENDER_TEXT_STYLE h, unsigned long i, CMyPos& j);
	HRESULT __stdcall Hooked_EndScene(IDirect3DDevice8* pDevice);
	//HRESULT __stdcall Hooked_Present(LPDIRECT3DDEVICE8 pDevice, CONST RECT* pSourceRect, CONST RECT* pDestRect, HWND hDestWindowOverride, CONST RGNDATA* pDirtyRegion);
};



#endif // !GRAPHIC_HOOK_H
