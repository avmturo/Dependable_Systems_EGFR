# Dependable_Systems_EGFR
EGFR Kidney calculator

Session Controller contains constant strings that should be used to lookup session variables.

The EGFR Formula relies on the following information:
- Age
- Ethnicity
- Gender

Therefore the patient details that we store must contain this information. Age changes year to year (obviously), so we
must store Date Of Birth to calculate the patients age at the time of calculation.

Ethnicity only affects the formula if the patient is of black ethnicity, therefore, we only care if the patient is black or not,
we do not care about storing other ethnicities, this allows us to store this information as a single bit.

1 = The patient is black
0 = The patient is another ethnicity

This will be stored as IsBlack in the database, this allows us to use the bit directly in the formula...

ethnicity modifier = 1 + (IsBlack * 0.210);


The same can be applied to gender as we have for ethnicity.

1 = The patient is female
0 = The patient is male

This will be stored as IsFemale in the database, again allowing us to use the bit directly in the formula...

gender modifier = 1 - (IsFemale * 0.258);

