#pragma once

// Required headers
#include <oaidl.h>
#include <ocidl.h>

// Define CLSID and IID for Flash ActiveX Control
const CLSID CLSID_ShockwaveFlash = { 0xD27CDB6E,0xAE6D,0x11CF,{0x96,0xB8,0x44,0x45,0x53,0x54,0x00,0x00} };
const IID IID_IShockwaveFlash = { 0xD27CDB6B,0xAE6D,0x11CF,{0x96,0xB8,0x44,0x45,0x53,0x54,0x00,0x00} };

// IShockwaveFlash interface definition
interface IShockwaveFlash : public IDispatch
{
public:
    virtual HRESULT STDMETHODCALLTYPE get_ReadyState(long* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE get_TotalFrames(long* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE get_Playing(VARIANT_BOOL* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE put_Playing(VARIANT_BOOL pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE get_Quality(int* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE put_Quality(int pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE get_ScaleMode(int* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE put_ScaleMode(int pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE get_AlignMode(int* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE put_AlignMode(int pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE get_BackgroundColor(long* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE put_BackgroundColor(long pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE get_Loop(VARIANT_BOOL* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE put_Loop(VARIANT_BOOL pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE get_Movie(BSTR* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE put_Movie(BSTR pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE get_FrameNum(long* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE put_FrameNum(long pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetZoomRect(long left, long top, long right, long bottom) = 0;
    virtual HRESULT STDMETHODCALLTYPE Zoom(int factor) = 0;
    virtual HRESULT STDMETHODCALLTYPE Pan(long x, long y, int mode) = 0;
    virtual HRESULT STDMETHODCALLTYPE Play() = 0;
    virtual HRESULT STDMETHODCALLTYPE Stop() = 0;
    virtual HRESULT STDMETHODCALLTYPE Back() = 0;
    virtual HRESULT STDMETHODCALLTYPE Forward() = 0;
    virtual HRESULT STDMETHODCALLTYPE Rewind() = 0;
    virtual HRESULT STDMETHODCALLTYPE StopPlay() = 0;
    virtual HRESULT STDMETHODCALLTYPE GotoFrame(long FrameNum) = 0;
    virtual HRESULT STDMETHODCALLTYPE CurrentFrame(long* FrameNum) = 0;
    virtual HRESULT STDMETHODCALLTYPE IsPlaying(VARIANT_BOOL* Playing) = 0;
    virtual HRESULT STDMETHODCALLTYPE PercentLoaded(long* pl) = 0;
    virtual HRESULT STDMETHODCALLTYPE FrameLoaded(long* pl) = 0;
    virtual HRESULT STDMETHODCALLTYPE FlashVersion(long* pl) = 0;
    virtual HRESULT STDMETHODCALLTYPE LoadMovie(int layer, BSTR url) = 0;
    virtual HRESULT STDMETHODCALLTYPE TGotoFrame(BSTR target, long FrameNum) = 0;
    virtual HRESULT STDMETHODCALLTYPE TGotoLabel(BSTR target, BSTR label) = 0;
    virtual HRESULT STDMETHODCALLTYPE TCurrentFrame(BSTR target, long* FrameNum) = 0;
    virtual HRESULT STDMETHODCALLTYPE TCurrentLabel(BSTR target, BSTR* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE CallFrame(long FrameNum) = 0;
    virtual HRESULT STDMETHODCALLTYPE CallLabel(BSTR label) = 0;
    virtual HRESULT STDMETHODCALLTYPE SetVariable(BSTR name, BSTR value) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetVariable(BSTR name, BSTR* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE TSetProperty(BSTR target, int property, BSTR value) = 0;
    virtual HRESULT STDMETHODCALLTYPE TGetProperty(BSTR target, int property, BSTR* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE TCallFrame(BSTR target, long FrameNum) = 0;
    virtual HRESULT STDMETHODCALLTYPE TCallLabel(BSTR target, BSTR label) = 0;
    virtual HRESULT STDMETHODCALLTYPE TSetPropertyNum(BSTR target, int property, double value) = 0;
    virtual HRESULT STDMETHODCALLTYPE TGetPropertyNum(BSTR target, int property, double* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE TGetPropertyAsNumber(BSTR target, int property, double* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetSwfVersion(long* pVal) = 0;
    virtual HRESULT STDMETHODCALLTYPE GetFlashVars(BSTR* pVal) = 0;
};
