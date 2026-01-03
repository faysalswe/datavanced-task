import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UnifiedSearchResult, OfficeSearchResult, PatientSearchResult, CaregiverSearchResult } from '../../models/models';
import { UnifiedSearchService } from '../../services/search.service';

@Component({
  selector: 'app-unified-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './unified-search.component.html',
  styleUrls: ['./unified-search.component.css']
})
export class UnifiedSearchComponent implements OnInit {
  keyword: string = '';
  currentPage: number = 1;
  pageSize: number = 10;
  searchResults: UnifiedSearchResult | null = null;
  hasSearched: boolean = false;

  constructor(private searchService: UnifiedSearchService) {}

  ngOnInit(): void {
    // Optional: Load initial results on component init
  }

  search(): void {
    if (!this.keyword.trim()) {
      this.searchResults = null;
      this.hasSearched = false;
      return;
    }

    this.currentPage = 1;
    this.performSearch();
  }

  performSearch(): void {
    this.searchService.unifiedSearch(this.keyword, this.currentPage, this.pageSize).subscribe({
      next: (result) => {
        this.searchResults = result;
        this.hasSearched = true;
      },
      error: (error) => {
        console.error('Error performing search:', error);
      }
    });
  }

  nextPage(): void {
    if (this.searchResults && this.currentPage < this.searchResults.totalPages) {
      this.currentPage++;
      this.performSearch();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.performSearch();
    }
  }

  onKeyUp(event: KeyboardEvent): void {
    if (event.key === 'Enter') {
      this.search();
    }
  }

  getFullName(firstName: string, lastName: string): string {
    return `${firstName} ${lastName}`;
  }

  getTotalResultsCount(): number {
    if (!this.searchResults) return 0;
    return this.searchResults.offices.length + 
           this.searchResults.patients.length + 
           this.searchResults.caregivers.length;
  }
}
