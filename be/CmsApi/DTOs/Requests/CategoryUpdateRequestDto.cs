using System.ComponentModel.DataAnnotations;

namespace CmsApi.DTOs.Requests
{
    /// <summary>
    /// Category update request DTO
    /// SRP: Sadece category update input data sorumluluğu
    /// PATCH pattern: Only updated fields are sent
    /// Business logic: Prevents accidental data overwrites
    /// </summary>
    public class CategoryUpdateRequestDto
    {
        /// <summary>
        /// Category name update
        /// Business rule: Must remain unique if changed
        /// Validation: Same as create but optional for PATCH
        /// </summary>
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Category name must be between 2 and 100 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_&]+$", 
            ErrorMessage = "Category name can only contain letters, numbers, spaces, hyphens, underscores, and ampersands")]
        public string? Name { get; set; }

        /// <summary>
        /// Category description update
        /// Business rule: Can be set to null to remove description
        /// </summary>
        [StringLength(500, ErrorMessage = "Category description cannot exceed 500 characters")]
        public string? Description { get; set; }

        /// <summary>
        /// Parent category change for reorganization
        /// Business rule: Cannot create circular references
        /// Null = move to root level
        /// </summary>
        public int? ParentCategoryId { get; set; }

        /// <summary>
        /// Slug update for URL optimization
        /// Business rule: Must remain unique, regenerated if Name changes
        /// SEO: Changing slug affects existing URLs
        /// </summary>
        [StringLength(100, ErrorMessage = "Category slug cannot exceed 100 characters")]
        [RegularExpression(@"^[a-z0-9\-]+$", 
            ErrorMessage = "Category slug can only contain lowercase letters, numbers, and hyphens")]
        public string? Slug { get; set; }

        /// <summary>
        /// Metadata update for enhanced categorization
        /// Business rule: Merge with existing metadata if partial update
        /// </summary>
        [StringLength(1000, ErrorMessage = "Category metadata cannot exceed 1000 characters")]
        public string? MetaData { get; set; }

        /// <summary>
        /// Display order change for reorganization
        /// Business rule: Affects sorting of sibling categories
        /// </summary>
        [Range(0, int.MaxValue, ErrorMessage = "Display order must be a positive number")]
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Active status toggle
        /// Business rule: Deactivating affects child categories
        /// Warning: Affects content visibility
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Indicates if this is a structure change (parent/order)
        /// Internal flag: Triggers hierarchy recalculation
        /// Performance: Avoid unnecessary tree rebuilds
        /// </summary>
        public bool IsStructureChange => ParentCategoryId.HasValue || DisplayOrder.HasValue;
    }
} 