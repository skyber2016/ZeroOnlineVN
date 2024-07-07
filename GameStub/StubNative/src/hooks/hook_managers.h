#ifndef HOOK_MANAGERS_H
#define HOOK_MANAGERS_H

#include "winsock/winsock_hook.h"
#include "graphic/graphic_hook.h"

namespace hook_managers
{
	inline bool _isInitialized = false;
	void Initialized();
	void Destroy();

};


#endif // !HOOK_MANAGERS_H


