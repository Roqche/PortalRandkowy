import { Photo } from './photo';

export interface User {
    /** Basi information */
    userId: number;
    userName: string;
    gender: string;
    age: number;
    zodiacSign: string;
    created: Date;
    lastActive: Date;
    city: string;
    country: string;

    /** Info tab */
    growth: string;
    eyeColor: string;
    hairColor: string;
    martialStatus: string;
    education: string;
    profession: string;
    children: string;
    languages: string;
    motto: string;
    description: string;
    personality: string;
    lookingFor: string;

    /** Hobby tab */
    interests: string;
    freeTime: string;
    sport: string;
    movies: string;
    music: string;
    iLike: string;
    idoNotLike: string;
    makesMeLaugh: string;
    itFeelsBestIn: string;
    friendsWouldDescribeMe: string;

    /** Photo tab */
    photos: Photo[];
    photoUrl: string;
}
