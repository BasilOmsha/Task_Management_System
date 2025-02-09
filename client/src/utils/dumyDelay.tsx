export const withLoadingDelay = (promise: Promise<any>, delay = 1500) => {
    if (import.meta.env.DEV) {
        return Promise.all([
            promise,
            new Promise(resolve => setTimeout(resolve, delay))
        ]).then(([module]) => module)
    }
    
    return promise
}