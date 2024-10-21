#ifndef DEFAULT_ACTION_H
#define DEFAULT_ACTION_H

#include "../action_factory.h"

class DefaultAction : public ActionFactory
{
public:
	int WINAPI Send(SOCKET s, const char* buf, int len, int flag) override;
	int WINAPI Recv(SOCKET s, char* buf, int len, int flag) override;
};


#endif // !DEFAULT_ACTION_H
