#ifndef INVOKE_HOOK_H
#define INVOKE_HOOK_H

#include <Windows.h>
#include "../../variables/type_definations.hpp"
#include "../../variables/global_variables.hpp"
#include <iostream>
#include <detours.h>

namespace invoke_hook
{
    inline HHOOK hHook = nullptr;
    LRESULT CALLBACK SetVariableHook(int nCode, WPARAM wParam, LPARAM lParam);
    void Init();
    void Destroy();

};



#endif // !INVOKE_HOOK_H
