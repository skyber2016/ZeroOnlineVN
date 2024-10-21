#ifndef TYPE_DEFINATIONS_HPP
#define TYPE_DEFINATIONS_HPP
#include "../../third_party/includes/d3d8.h"

#include <winsock.h>

typedef int (WINAPI* send_t)(SOCKET, const char*, int, int);
typedef int (WINAPI* recv_t)(SOCKET, char*, int, int);
typedef int (WINAPI* connect_t)(SOCKET, const sockaddr*, int);
typedef int(__cdecl* GameFontCreate_t)(void);
typedef void* (__cdecl* CreateNew_t)();
typedef unsigned long(__thiscall* CalcCardChkSum_t)(void*, int, int, int);
typedef unsigned long(__thiscall* CalcUserChkSum_t)(void* thisPointer, int, unsigned __int64, int);
typedef unsigned long(__thiscall* CalcUserXChkSum_t)(void* thisPointer, unsigned __int64, int, int, int);

enum RENDER_TEXT_STYLE {
	STYLE_NORMAL,
	STYLE_BOLD,
	STYLE_ITALIC,
	STYLE_UNDERLINE
};

struct CMyPos {
	int x; // Tọa độ x
	int y; // Tọa độ y
};


typedef void(__cdecl* ShowString_t)		(int, int, unsigned long, const char*, const char*, int, int, RENDER_TEXT_STYLE, unsigned long, CMyPos*);
typedef void(__cdecl* ShowTString_t)	(int, int, unsigned long, char const*, char const*, int, int, RENDER_TEXT_STYLE, unsigned long, CMyPos*);
typedef void(__cdecl* ShowStringEx_t)	(int, int, unsigned long, char const*, char const*, int, int, RENDER_TEXT_STYLE, unsigned long, CMyPos*);
typedef HRESULT(APIENTRY* Reset_t) (LPDIRECT3DDEVICE8 pDevice, D3DPRESENT_PARAMETERS* pPresentationParameters);
typedef HRESULT(APIENTRY* Present_t)(LPDIRECT3DDEVICE8 pDevice, CONST RECT* pSourceRect, CONST RECT* pDestRect, HWND hDestWindowOverride, CONST RGNDATA* pDirtyRegion);
typedef HRESULT(__stdcall* EndScene_t)(IDirect3DDevice8* pDevice);


#endif // !TYPE_DEFINATIONS_H
