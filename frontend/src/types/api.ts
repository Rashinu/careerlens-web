export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
}

export interface UserProfileDto {
  id: string;
  email: string;
  firstName: string | null;
  lastName: string | null;
  plan: string;
  createdAt: string;
}

export interface CvAnalysisListItemDto {
  id: string;
  originalFileName: string;
  status: 'Uploaded' | 'TextExtracted' | 'Analyzed' | 'Failed';
  createdAt: string;
}

export interface CvAnalysisDto {
  id: string;
  originalFileName: string;
  status: string;
  parsedData: string | null;
  createdAt: string;
}

export interface RoadmapDto {
  id: string;
  cvAnalysisId: string;
  targetPosition: string;
  currentScore: number;
  gapAnalysis: string;
  recommendations: string;
  generatedAt: string;
}

export interface SalaryBenchmarkDto {
  p25: number;
  p50: number;
  p75: number;
  sampleCount: number;
  position: string;
  city: string;
  yearsOfExperience: number;
  isInflationAdjusted: boolean;
}

export interface DashboardDto {
  totalCvAnalyses: number;
  completedAnalyses: number;
  latestCvStatus: string | null;
  latestRoadmapScore: number | null;
  latestTargetPosition: string | null;
}

export interface ApiResponse<T> {
  success: boolean;
  data: T;
  error: string | null;
}
