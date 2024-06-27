#ifndef HOOK_MANAGERS_H
#define HOOK_MANAGERS_H

#include "winsock/winsock_hook.h"

namespace hook_managers
{
	inline bool _isInitialized = false;
	void initialized();
	void destroy();

};


#endif // !HOOK_MANAGERS_H


