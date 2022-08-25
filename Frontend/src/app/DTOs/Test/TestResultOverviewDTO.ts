import { TestResultOverviewUserDTO } from "./TestResultOverviewUserDTO";

export interface TestResultOverviewDTO {
    testName: string;
    pointsPossible: number;
    maxTime: number;
    results: TestResultOverviewUserDTO[];
}