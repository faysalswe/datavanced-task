import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Patient, Caregiver, SearchRequest } from '../../models/models';
import { PatientService } from '../../services/patient.service';
import { CaregiverService } from '../../services/caregiver.service';

@Component({
  selector: 'app-patient-grid',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './patient-grid.component.html',
  styleUrls: ['./patient-grid.component.css']
})
export class PatientGridComponent implements OnInit {
  patients: Patient[] = [];
  availableCaregivers: Caregiver[] = [];
  totalCount = 0;
  currentPage = 1;
  pageSize = 10;
  keyword = '';
  sortBy = 'lastName';
  isDescending = false;
  
  editingPatient: Patient | null = null;
  showAddForm = false;
  newPatient: any = {
    firstName: '',
    lastName: '',
    dateOfBirth: '',
    phone: '',
    email: '',
    address: '',
    officeId: 1,
    caregiverIds: []
  };

  constructor(
    private patientService: PatientService,
    private caregiverService: CaregiverService
  ) {}

  ngOnInit(): void {
    this.loadPatients();
    this.loadCaregivers();
  }

  loadPatients(): void {
    const request: SearchRequest = {
      keyword: this.keyword || undefined,
      page: this.currentPage,
      pageSize: this.pageSize,
      sortBy: this.sortBy,
      isDescending: this.isDescending
    };

    this.patientService.search(request).subscribe({
      next: (result) => {
        this.patients = result.items;
        this.totalCount = result.totalCount;
      },
      error: (error) => console.error('Error loading patients:', error)
    });
  }

  loadCaregivers(): void {
    const request: SearchRequest = {
      page: 1,
      pageSize: 100,
      sortBy: 'lastName',
      isDescending: false
    };

    this.caregiverService.search(request).subscribe({
      next: (result) => {
        this.availableCaregivers = result.items;
      },
      error: (error) => console.error('Error loading caregivers:', error)
    });
  }

  search(): void {
    this.currentPage = 1;
    this.loadPatients();
  }

  sort(field: string): void {
    if (this.sortBy === field) {
      this.isDescending = !this.isDescending;
    } else {
      this.sortBy = field;
      this.isDescending = false;
    }
    this.loadPatients();
  }

  nextPage(): void {
    if (this.currentPage * this.pageSize < this.totalCount) {
      this.currentPage++;
      this.loadPatients();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadPatients();
    }
  }

  startEdit(patient: Patient): void {
    this.editingPatient = { ...patient };
  }

  cancelEdit(): void {
    this.editingPatient = null;
  }

  saveEdit(): void {
    if (this.editingPatient) {
      const updateData = {
        id: this.editingPatient.id,
        firstName: this.editingPatient.firstName,
        lastName: this.editingPatient.lastName,
        dateOfBirth: this.editingPatient.dateOfBirth,
        phone: this.editingPatient.phone,
        email: this.editingPatient.email,
        address: this.editingPatient.address,
        officeId: this.editingPatient.officeId,
        caregiverIds: this.editingPatient.caregivers.map(c => c.id)
      };

      this.patientService.update(this.editingPatient.id, updateData).subscribe({
        next: () => {
          this.editingPatient = null;
          this.loadPatients();
        },
        error: (error) => console.error('Error updating patient:', error)
      });
    }
  }

  deletePatient(id: number): void {
    if (confirm('Are you sure you want to delete this patient?')) {
      this.patientService.delete(id).subscribe({
        next: () => this.loadPatients(),
        error: (error) => console.error('Error deleting patient:', error)
      });
    }
  }

  toggleAddForm(): void {
    this.showAddForm = !this.showAddForm;
    if (!this.showAddForm) {
      this.resetNewPatient();
    }
  }

  addPatient(): void {
    this.patientService.create(this.newPatient).subscribe({
      next: () => {
        this.toggleAddForm();
        this.loadPatients();
      },
      error: (error) => console.error('Error creating patient:', error)
    });
  }

  resetNewPatient(): void {
    this.newPatient = {
      firstName: '',
      lastName: '',
      dateOfBirth: '',
      phone: '',
      email: '',
      address: '',
      officeId: 1,
      caregiverIds: []
    };
  }

  toggleCaregiver(patientCaregivers: Caregiver[], caregiverId: number): void {
    const index = patientCaregivers.findIndex(c => c.id === caregiverId);
    if (index >= 0) {
      patientCaregivers.splice(index, 1);
    } else {
      const caregiver = this.availableCaregivers.find(c => c.id === caregiverId);
      if (caregiver) {
        patientCaregivers.push(caregiver);
      }
    }
  }

  toggleNewPatientCaregiver(caregiverId: number): void {
    const index = this.newPatient.caregiverIds.indexOf(caregiverId);
    if (index >= 0) {
      this.newPatient.caregiverIds.splice(index, 1);
    } else {
      this.newPatient.caregiverIds.push(caregiverId);
    }
  }

  isEditingPatient(patient: Patient): boolean {
    return this.editingPatient?.id === patient.id;
  }

  isCaregiverSelected(patientCaregivers: Caregiver[], caregiverId: number): boolean {
    return patientCaregivers.some(c => c.id === caregiverId);
  }

  isNewPatientCaregiverSelected(caregiverId: number): boolean {
    return this.newPatient.caregiverIds.includes(caregiverId);
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }
}
