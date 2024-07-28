import { User } from "./user";

// Using a class because we want to create an instance of UserParams
export class UserParams
{
    gender: string;
    minAge = 18;
    maxAge = 99;
    pageNumber = 1;
    pageSize = 5;
    orderBy = 'lastActive';

    constructor(user:User | null)
    {
        // If the user's gender is female we will set our the gender FILTER to male, otherwise female 
        this.gender = user?.gender === 'female' ?  'male' : 'female';

    }
}