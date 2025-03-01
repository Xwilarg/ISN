VAR firstSpeak = false
VAR insight = 0
{
	- firstSpeak:
        You approch toward a young lady dressed in a long green and white robe wearing the insign of Io, the temple of life
        She look at you shyly, her eyes darting around, awkwardly searching for words
		Ah erm, well, hello you're here for the tower? #speaker saria #name ???
        Yes of course you are, Saria come on...#name none
        Erm anyway, my name is Saria, I'm a prietress of life, nice to meet you!
}
*	[What can you do?]
	Saria puff out her chest proudly #speaker none
    Io is the god of life and prosperity, as her disciples we are taught to cure wound and ailment #speaker saria
    As a prietress I am able to instill back like energy, there is however limits on what I can do
    Life magic is not able to regrow an arm or give you back your blood, so be careful okay?
*	[Join me]
	Thank you! lead the way I will be behind you #speaker saria #action recruit
*	[I will see you later]
	Y-yes I will be here if you're looking for help #speaker saria
-> END