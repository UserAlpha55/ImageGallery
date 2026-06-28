let sortableInstance = null;

export function initSortable(containerId, dotNetHelper, callbackName) {
    const container = document.getElementById(containerId);
    if (!container) return;

    if (sortableInstance) {
        sortableInstance.destroy();
    }

    import('https://cdn.jsdelivr.net/npm/sortablejs@1.15.6/Sortable.min.js').then((Sortable) => {
        sortableInstance = new Sortable.default(container, {
            animation: 200,
            handle: '.drag-handle',
            ghostClass: 'sortable-ghost',
            onEnd: function (evt) {
                const items = container.querySelectorAll('.sortable-item');
                const imageIds = Array.from(items).map(item => item.getAttribute('data-image-id'));
                dotNetHelper.invokeMethodAsync(callbackName, imageIds);
            }
        });
    });
}

export function destroySortable() {
    if (sortableInstance) {
        sortableInstance.destroy();
        sortableInstance = null;
    }
}
