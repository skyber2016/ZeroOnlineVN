#ifndef DEFAULT_ACTION_H
#define DEFAULT_ACTION_H

#include "../action_factory.h"

class DefaultAction : public ActionFactory
{
public:
	int WINAPI Send(const char* buf, int len, int flag) override;
	int WINAPI Recv(char* buf, int len, int flag) override;
};


#endif // !DEFAULT_ACTION_H
