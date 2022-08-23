export interface TestAdjustUserPointDTO {
    userId: string;
    testId: string;
    questionId: string;
    pointsScored: number;
    isCorrect: boolean;
}