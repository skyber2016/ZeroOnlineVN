#ifndef BASE_REQUEST_HPP
#define BASE_REQUEST_HPP

#include "data_type.hpp"

struct BaseRequest
{
	unsigned short len;
	DataType type;
};

#endif // !BASE_REQUEST_HPP
