#ifndef Third_H
#define Third_H

namespace SUB
{
	template<typename T1, typename T2, typename T3>
	struct Third
	{
		T1 first;
		T2 second;
		T3 third;

		Third(Third const&) = default;
		Third(Third &&) = default;

		Third& operator=(Third const& p);
		Third& operator=(Third && p);

		Third(const T1& _first, const T2& _second, const T3& _third) :
			first(_first),
			second(_second),
			third(_third)
		{}

		template<typename U1, typename U2, typename U3>
		static Third make_third(const U1& _first, const U2& _second, const U3& _third)
		{
			return Third(_first, _second, _third);
		}
	};
}

#endif
