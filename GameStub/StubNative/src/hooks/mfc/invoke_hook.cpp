#include "invoke_hook.h"
namespace invoke_hook {
    void Init() {
    }
    void Destroy() {
        UnhookWindowsHookEx(hHook);
    }
}