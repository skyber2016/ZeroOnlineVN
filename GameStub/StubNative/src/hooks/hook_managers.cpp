#include "hook_managers.h"
namespace hook_managers
{
	void initialized()
	{
		winsock_hook::init();
		_isInitialized = true;
	}

	void destroy()
	{
		if (_isInitialized)
		{
			winsock_hook::destroy();
		}
		_isInitialized = false;
	}
}