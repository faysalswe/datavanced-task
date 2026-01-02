export interface Patient {
  id: number;
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  phone: string;
  email: string;
  address: string;
  officeId: number;
  officeName: string;
  caregivers: Caregiver[];
}

export interface CreatePatient {
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  phone: string;
  email: string;
  address: string;
  officeId: number;
  caregiverIds: number[];
}

export interface UpdatePatient {
  id: number;
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  phone: string;
  email: string;
  address: string;
  officeId: number;
  caregiverIds: number[];
}

export interface Caregiver {
  id: number;
  firstName: string;
  lastName: string;
  phone: string;
  email: string;
  specialization: string;
  officeId: number;
  officeName: string;
}

export interface SearchResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface SearchRequest {
  keyword?: string;
  page: number;
  pageSize: number;
  sortBy?: string;
  isDescending: boolean;
}
