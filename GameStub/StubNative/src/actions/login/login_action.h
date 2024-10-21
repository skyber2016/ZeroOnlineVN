#ifndef LOGIN_ACTION_H
#define LOGIN_ACTION_H

#include "../action_factory.h"

class LoginAction : public ActionFactory
{
private:
public:
	int WINAPI Send(SOCKET s, const char* buf, int len, int flag) override;
	int WINAPI Recv(SOCKET s, char* buf, int len, int flag) override;
};

#endif // !LOGIN_ACTION_H
